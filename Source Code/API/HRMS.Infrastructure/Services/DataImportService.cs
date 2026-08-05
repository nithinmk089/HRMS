using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Infrastructure.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly string _connectionString;

        public DataImportService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public byte[] GenerateTemplateCsv(string entityType)
        {
            string headers = (entityType ?? "").ToLower() switch
            {
                "employees" => "EmployeeCode,FirstName,LastName,PersonalEmail,MobileNumber,Gender,DepartmentCode,DesignationCode,LocationCode,Status",
                "companies" => "CompanyCode,CompanyName,LegalName,TaxNumber,Email,Phone,Website",
                "business-units" or "businessunits" => "BusinessUnitCode,BusinessUnitName,CompanyCode",
                "departments" => "DepartmentCode,DepartmentName,Description,ParentDepartmentCode",
                "designations" => "DesignationCode,DesignationName,Description,JobGrade",
                "locations" or "physical-locations" => "LocationCode,LocationName,Address,City,StateCode,CountryCode,PostalCode",
                "cost-centers" or "costcenters" => "CostCenterCode,CostCenterName,Description",
                "leave-types" or "leavetypes" => "LeaveCode,LeaveName,IsPaid,IsAccrualBased",
                "shifts" or "work-shifts" => "ShiftCode,ShiftName,ShiftType,StartTime,EndTime",
                "salary-components" or "salarycomponents" => "ComponentCode,ComponentName,ComponentType,CalculationMethod",
                "expense-categories" or "expensecategories" => "CategoryCode,CategoryName,MaximumLimit",
                "asset-categories" or "assetcategories" => "CategoryCode,CategoryName,Description",
                "skills" or "skill-catalog" => "SkillCode,SkillName,SkillCategory",
                "leave-balances" => "EmployeeCode,LeaveTypeCode,TotalAccruedDays,CarryForwardDays",
                "attendance" => "EmployeeCode,AttendanceDate,PunchInTime,PunchOutTime,Status",
                _ => "Code,Name,Description"
            };

            string sampleRow = (entityType ?? "").ToLower() switch
            {
                "employees" => "EMP101,John,Doe,john.doe@company.com,+1-555-0199,Male,DEPT-IT,DESG-DEV,LOC-NY,Active",
                "companies" => "COMP-001,Acme Tech,Acme Tech LLC,TAX-9988,contact@acme.com,+1-555-0100,https://acme.com",
                "business-units" or "businessunits" => "BU-ENG,Engineering Operations,COMP-001",
                "departments" => "DEPT-IT,Information Technology,Software & Infrastructure,DEPT-CORP",
                "designations" => "DESG-DEV,Senior Software Engineer,Engineering Role,Grade-5",
                "locations" or "physical-locations" => "LOC-NY,New York Headquarters,100 Wall Street,New York,NY,US,10005",
                "cost-centers" or "costcenters" => "CC-ENG,Engineering Cost Center,R&D Expense Tracking",
                "leave-types" or "leavetypes" => "LEAVE-ANNUAL,Annual Paid Leave,1,1",
                "shifts" or "work-shifts" => "SHIFT-GEN,General Shift,Regular,09:00:00,18:00:00",
                "salary-components" or "salarycomponents" => "BASIC,Basic Pay,Earning,Fixed",
                "expense-categories" or "expensecategories" => "EXP-FLIGHT,Air Travel Expense,1500.00",
                "asset-categories" or "assetcategories" => "CAT-LAP,Laptops & Notebooks,Portable Workstations",
                "skills" or "skill-catalog" => "SKILL-CSHARP,C# .NET Development,Software Development",
                "leave-balances" => "EMP101,LEAVE-ANNUAL,20.0,5.0",
                "attendance" => "EMP101,2026-07-20,09:00:00,18:00:00,Present",
                _ => "SAMPLE-01,Sample Item Name,Sample description text"
            };

            var csvContent = $"{headers}\n{sampleRow}\n";
            return Encoding.UTF8.GetBytes(csvContent);
        }

        public async Task<ImportResultDto> ImportEmployeesAsync(long tenantId, long userId, Stream fileStream, string fileName)
        {
            var result = new ImportResultDto { EntityType = "Employees", Success = true };
            var rows = ReadCsvRows(fileStream);
            result.TotalRows = rows.Count;

            if (rows.Count == 0)
            {
                result.Success = false;
                result.Message = "Uploaded file is empty or formatted incorrectly.";
                return result;
            }

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                int rowNum = i + 2; // Line 1 is header

                string empCode = GetVal(row, "EmployeeCode", "EmpCode", "Code");
                string firstName = GetVal(row, "FirstName", "First Name", "Name");
                string lastName = GetVal(row, "LastName", "Last Name");
                string email = GetVal(row, "PersonalEmail", "Email", "WorkEmail");
                string mobile = GetVal(row, "MobileNumber", "Mobile", "Phone");

                if (string.IsNullOrWhiteSpace(empCode) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(email))
                {
                    result.FailedCount++;
                    result.Errors.Add(new ImportRowErrorDto
                    {
                        RowIndex = rowNum,
                        Identifier = empCode ?? $"Row {rowNum}",
                        ErrorMessage = "Required fields missing (EmployeeCode, FirstName, PersonalEmail required)."
                    });
                    continue;
                }

                try
                {
                    await connection.ExecuteAsync(
                        @"IF EXISTS (SELECT 1 FROM hr.Employee WHERE TenantID = @TenantID AND EmployeeCode = @EmployeeCode)
                          BEGIN
                              UPDATE hr.Employee 
                              SET FirstName = @FirstName, LastName = @LastName, PersonalEmail = @PersonalEmail, MobileNumber = @MobileNumber, ModifiedBy = @UserId, ModifiedDate = GETUTCDATE()
                              WHERE TenantID = @TenantID AND EmployeeCode = @EmployeeCode;
                          END
                          ELSE
                          BEGIN
                              INSERT INTO hr.Employee (TenantID, EmployeeCode, EmployeeNumber, FirstName, LastName, PersonalEmail, MobileNumber, [Status], CreatedBy, IsDeleted)
                              VALUES (@TenantID, @EmployeeCode, @EmployeeCode, @FirstName, @LastName, @PersonalEmail, @MobileNumber, 'Active', @UserId, 0);
                          END

                          -- Auto-provision User Login Account in security.[User] if not existing
                          DECLARE @EmpID BIGINT;
                          SELECT @EmpID = EmployeeID FROM hr.Employee WHERE TenantID = @TenantID AND EmployeeCode = @EmployeeCode;

                          IF NOT EXISTS (SELECT 1 FROM security.[User] WHERE TenantID = @TenantID AND (EmployeeID = @EmpID OR Email = @PersonalEmail OR UserName = @PersonalEmail))
                          BEGIN
                              INSERT INTO security.[User] (TenantID, EmployeeID, UserName, Email, PasswordHash, PasswordSalt, IsLocked, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                              VALUES (@TenantID, @EmpID, @PersonalEmail, @PersonalEmail, 'AQAAAAEAACcQAAAAEHASH', 'salt_placeholder', 0, @UserId, GETUTCDATE(), 0, 1);
                          END
                          ELSE
                          BEGIN
                              UPDATE security.[User]
                              SET EmployeeID = @EmpID, UserName = @PersonalEmail, Email = @PersonalEmail, ModifiedBy = @UserId, ModifiedDate = GETUTCDATE()
                              WHERE TenantID = @TenantID AND (EmployeeID = @EmpID OR Email = @PersonalEmail);
                          END",
                        new {
                            TenantID = tenantId,
                            EmployeeCode = empCode,
                            FirstName = firstName,
                            LastName = lastName ?? "",
                            PersonalEmail = email,
                            MobileNumber = mobile ?? "555-0100",
                            UserId = userId
                        }
                    );
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.Errors.Add(new ImportRowErrorDto
                    {
                        RowIndex = rowNum,
                        Identifier = empCode,
                        ErrorMessage = ex.Message
                    });
                }
            }

            result.Message = $"Employee Import Completed. Processed: {result.TotalRows}, Success: {result.SuccessCount}, Failed: {result.FailedCount}";
            return result;
        }

        public async Task<ImportResultDto> ImportMasterDataAsync(long tenantId, long userId, string entityType, Stream fileStream, string fileName)
        {
            var result = new ImportResultDto { EntityType = entityType, Success = true };

            try
            {
                var rows = ReadCsvRows(fileStream);
                result.TotalRows = rows.Count;

                if (rows.Count == 0)
                {
                    result.Success = false;
                    result.Message = "Uploaded file is empty or missing headers.";
                    return result;
                }

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                string normalizedEntity = (entityType ?? "").ToLower().Replace("_", "-").Replace(" ", "-");

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    int rowNum = i + 2;
                    string code = GetVal(row, "BusinessUnitCode", "BusinessUnit_Code", "BusinessUnit", "BUCode", "CostCenterCode", "CostCenter_Code", "CostCenter", "Code", "DepartmentCode", "DesignationCode", "LocationCode", "CompanyCode", "CategoryCode", "LeaveCode", "LeaveTypeCode", "ShiftCode", "ComponentCode", "SalaryComponentCode", "SkillCode");
                    string name = GetVal(row, "BusinessUnitName", "BusinessUnit_Name", "BUName", "CostCenterName", "CostCenter_Name", "Name", "DepartmentName", "DesignationName", "LocationName", "CompanyName", "CategoryName", "LeaveName", "LeaveTypeName", "ShiftName", "ComponentName", "SalaryComponentName", "SkillName");

                    if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
                    {
                        result.FailedCount++;
                        result.Errors.Add(new ImportRowErrorDto
                        {
                            RowIndex = rowNum,
                            Identifier = code ?? $"Row {rowNum}",
                            ErrorMessage = "Master Data code and name are required."
                        });
                        continue;
                    }

                    try
                    {
                        switch (normalizedEntity)
                        {
                            case "departments":
                            case "department":
                                string parentCode = GetVal(row, "ParentDepartmentCode", "ParentCode", "Parent", "ParentDepartment");
                                await connection.ExecuteAsync(
                                    @"DECLARE @BuID BIGINT = (SELECT TOP 1 BusinessUnitID FROM organization.BusinessUnit WHERE TenantID = @TenantID AND IsDeleted = 0);
                                      IF @BuID IS NULL
                                      BEGIN
                                          DECLARE @CompID BIGINT = (SELECT TOP 1 CompanyID FROM security.Company WHERE TenantID = @TenantID AND IsDeleted = 0);
                                          IF @CompID IS NULL SET @CompID = 1;
                                          INSERT INTO organization.BusinessUnit (TenantID, CompanyID, BusinessUnitCode, BusinessUnitName, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @CompID, 'BU-DEFAULT', 'General Operations BU', @UserId, 0);
                                          SET @BuID = SCOPE_IDENTITY();
                                      END

                                      DECLARE @ParentID BIGINT = NULL;
                                      IF @ParentCode IS NOT NULL AND @ParentCode <> ''
                                      BEGIN
                                          SELECT TOP 1 @ParentID = DepartmentID 
                                          FROM organization.Department 
                                          WHERE TenantID = @TenantID AND DepartmentCode = @ParentCode AND IsDeleted = 0;
                                      END

                                      IF NOT EXISTS (SELECT 1 FROM organization.Department WHERE TenantID = @TenantID AND DepartmentCode = @Code)
                                      BEGIN
                                          INSERT INTO organization.Department (TenantID, BusinessUnitID, DepartmentCode, DepartmentName, ParentDepartmentID, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @BuID, @Code, @Name, @ParentID, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE organization.Department 
                                          SET DepartmentName = @Name, 
                                              ParentDepartmentID = ISNULL(@ParentID, ParentDepartmentID)
                                          WHERE TenantID = @TenantID AND DepartmentCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, ParentCode = parentCode, UserId = userId });
                                break;

                            case "designations":
                            case "designation":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM organization.Designation WHERE TenantID = @TenantID AND DesignationCode = @Code)
                                      BEGIN
                                          INSERT INTO organization.Designation (TenantID, DesignationCode, DesignationName, Grade, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @Grade, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE organization.Designation
                                          SET DesignationName = @Name, Grade = ISNULL(@Grade, Grade)
                                          WHERE TenantID = @TenantID AND DesignationCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, Grade = GetVal(row, "JobGrade", "Grade"), UserId = userId });
                                break;

                            case "locations":
                            case "location":
                            case "physical-locations":
                                string countryCode = SafeSubstring(GetVal(row, "CountryCode", "Country", "Country/Nation", "Nation"), 10);
                                string stateCode = SafeSubstring(GetVal(row, "StateCode", "State", "State/Province", "Province", "Region"), 10);
                                string city = GetVal(row, "City", "Town") ?? "";

                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM organization.Location WHERE TenantID = @TenantID AND LocationCode = @Code)
                                      BEGIN
                                          INSERT INTO organization.Location (TenantID, LocationCode, LocationName, CountryCode, StateCode, City, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @CountryCode, @StateCode, @City, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE organization.Location
                                          SET LocationName = @Name, 
                                              CountryCode = @CountryCode, 
                                              StateCode = @StateCode, 
                                              City = @City
                                          WHERE TenantID = @TenantID AND LocationCode = @Code;
                                      END",
                                    new { 
                                        TenantID = tenantId, 
                                        Code = code, 
                                        Name = name, 
                                        CountryCode = countryCode, 
                                        StateCode = stateCode,
                                        City = city, 
                                        UserId = userId 
                                    });
                                break;

                            case "cost-centers":
                            case "cost-center":
                            case "costcenters":
                            case "costcenter":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM organization.CostCenter WHERE TenantID = @TenantID AND CostCenterCode = @Code)
                                      BEGIN
                                          INSERT INTO organization.CostCenter (TenantID, CostCenterCode, CostCenterName, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE organization.CostCenter
                                          SET CostCenterName = @Name
                                          WHERE TenantID = @TenantID AND CostCenterCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, UserId = userId });
                                break;

                            case "business-units":
                            case "business-unit":
                            case "businessunits":
                            case "businessunit":
                                await connection.ExecuteAsync(
                                    @"DECLARE @CompID BIGINT = (SELECT TOP 1 CompanyID FROM security.Company WHERE TenantID = @TenantID AND IsDeleted = 0);
                                      IF @CompID IS NULL SET @CompID = 1;
                                      IF NOT EXISTS (SELECT 1 FROM organization.BusinessUnit WHERE TenantID = @TenantID AND BusinessUnitCode = @Code)
                                      BEGIN
                                          INSERT INTO organization.BusinessUnit (TenantID, CompanyID, BusinessUnitCode, BusinessUnitName, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @CompID, @Code, @Name, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE organization.BusinessUnit SET BusinessUnitName = @Name WHERE TenantID = @TenantID AND BusinessUnitCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, UserId = userId });
                                break;

                            case "companies":
                            case "company":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM security.Company WHERE TenantID = @TenantID AND CompanyCode = @Code)
                                      BEGIN
                                          INSERT INTO security.Company (TenantID, CompanyCode, CompanyName, LegalName, TaxNumber, Email, Phone, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @LegalName, @TaxNumber, @Email, @Phone, @UserId, 0);
                                      END",
                                    new { 
                                        TenantID = tenantId, 
                                        Code = code, 
                                        Name = name, 
                                        LegalName = GetVal(row, "LegalName") ?? name,
                                        TaxNumber = GetVal(row, "TaxNumber") ?? "TAX-001",
                                        Email = GetVal(row, "Email") ?? "contact@company.com",
                                        Phone = GetVal(row, "Phone") ?? "555-0100",
                                        UserId = userId 
                                    });
                                break;

                            case "leave-types":
                            case "leave-type":
                            case "leavetypes":
                            case "leavetype":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM leave.LeaveType WHERE TenantID = @TenantID AND LeaveCode = @Code)
                                      BEGIN
                                          INSERT INTO leave.LeaveType (TenantID, LeaveCode, LeaveName, IsPaid, IsAccrualBased, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, 1, 1, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE leave.LeaveType SET LeaveName = @Name WHERE TenantID = @TenantID AND LeaveCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, UserId = userId });
                                break;

                            case "shifts":
                            case "shift":
                            case "work-shifts":
                                string shiftType = GetVal(row, "ShiftType", "Type") ?? "Regular";
                                string startTimeStr = GetVal(row, "StartTime", "Start", "InTime") ?? "09:00:00";
                                string endTimeStr = GetVal(row, "EndTime", "End", "OutTime") ?? "18:00:00";

                                if (!TimeSpan.TryParse(startTimeStr, out TimeSpan startTime))
                                {
                                    startTime = new TimeSpan(9, 0, 0);
                                }

                                if (!TimeSpan.TryParse(endTimeStr, out TimeSpan endTime))
                                {
                                    endTime = new TimeSpan(18, 0, 0);
                                }

                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM attendance.Shift WHERE TenantID = @TenantID AND ShiftCode = @Code)
                                      BEGIN
                                          INSERT INTO attendance.Shift (TenantID, ShiftCode, ShiftName, ShiftType, StartTime, EndTime, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @ShiftType, @StartTime, @EndTime, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE attendance.Shift 
                                          SET ShiftName = @Name, ShiftType = @ShiftType, StartTime = @StartTime, EndTime = @EndTime 
                                          WHERE TenantID = @TenantID AND ShiftCode = @Code;
                                      END",
                                    new { 
                                        TenantID = tenantId, 
                                        Code = code, 
                                        Name = name, 
                                        ShiftType = shiftType,
                                        StartTime = startTime,
                                        EndTime = endTime,
                                        UserId = userId 
                                    });
                                break;

                            case "salary-components":
                            case "salary-component":
                            case "salarycomponents":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM payroll.SalaryComponent WHERE TenantID = @TenantID AND ComponentCode = @Code)
                                      BEGIN
                                          INSERT INTO payroll.SalaryComponent (TenantID, ComponentCode, ComponentName, ComponentType, CalculationMethod, TaxableFlag, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, 'Earning', 'Fixed', 1, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE payroll.SalaryComponent SET ComponentName = @Name WHERE TenantID = @TenantID AND ComponentCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, UserId = userId });
                                break;

                            case "expense-categories":
                            case "expense-category":
                            case "expensecategories":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM travel.ExpenseCategory WHERE TenantID = @TenantID AND CategoryCode = @Code)
                                      BEGIN
                                          INSERT INTO travel.ExpenseCategory (TenantID, CategoryCode, CategoryName, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE travel.ExpenseCategory SET CategoryName = @Name WHERE TenantID = @TenantID AND CategoryCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, UserId = userId });
                                break;

                            case "asset-categories":
                            case "asset-category":
                            case "assetcategories":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM asset.AssetCategory WHERE TenantID = @TenantID AND CategoryCode = @Code)
                                      BEGIN
                                          INSERT INTO asset.AssetCategory (TenantID, CategoryCode, CategoryName, Description, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, @Desc, @UserId, 0);
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, Desc = GetVal(row, "Description"), UserId = userId });
                                break;

                            case "skills":
                            case "skill":
                            case "skill-catalog":
                                await connection.ExecuteAsync(
                                    @"IF NOT EXISTS (SELECT 1 FROM learning.Skill WHERE TenantID = @TenantID AND SkillCode = @Code)
                                      BEGIN
                                          INSERT INTO learning.Skill (TenantID, SkillCode, SkillName, SkillCategory, CreatedBy, IsDeleted)
                                          VALUES (@TenantID, @Code, @Name, 'Technical', @UserId, 0);
                                      END
                                      ELSE
                                      BEGIN
                                          UPDATE learning.Skill SET SkillName = @Name WHERE TenantID = @TenantID AND SkillCode = @Code;
                                      END",
                                    new { TenantID = tenantId, Code = code, Name = name, UserId = userId });
                                break;

                            default:
                                result.FailedCount++;
                                result.Errors.Add(new ImportRowErrorDto { RowIndex = rowNum, Identifier = code, ErrorMessage = "Unsupported master data entity type." });
                                continue;
                        }
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.Errors.Add(new ImportRowErrorDto { RowIndex = rowNum, Identifier = code, ErrorMessage = ex.Message });
                }
            }

            // Pass 2: If importing departments, resolve and update all ParentDepartmentIDs 
            if (normalizedEntity.Equals("departments", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    string code = GetVal(row, "Code", "DepartmentCode");
                    string parentCode = GetVal(row, "ParentDepartmentCode", "ParentCode", "Parent", "ParentDepartment");
                    if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(parentCode))
                    {
                        await connection.ExecuteAsync(
                            @"UPDATE d
                              SET d.ParentDepartmentID = p.DepartmentID
                              FROM organization.Department d
                              INNER JOIN organization.Department p ON p.TenantID = d.TenantID AND p.DepartmentCode = @ParentCode AND p.IsDeleted = 0
                              WHERE d.TenantID = @TenantID AND d.DepartmentCode = @Code AND d.IsDeleted = 0;",
                            new { TenantID = tenantId, Code = code, ParentCode = parentCode });
                    }
                }
            }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Server error processing file import: {ex.Message}";
            }

            result.Message = $"Master Data Import ({entityType}) Completed. Total: {result.TotalRows}, Success: {result.SuccessCount}, Failed: {result.FailedCount}";
            return result;
        }

        public async Task<ImportResultDto> ImportOperationalDataAsync(long tenantId, long userId, string entityType, Stream fileStream, string fileName)
        {
            var result = new ImportResultDto { EntityType = entityType, Success = true };
            var rows = ReadCsvRows(fileStream);
            result.TotalRows = rows.Count;

            if (rows.Count == 0)
            {
                result.Success = false;
                result.Message = "Uploaded operational import file is empty.";
                return result;
            }

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                int rowNum = i + 2;
                string empCode = GetVal(row, "EmployeeCode", "EmpCode");

                if (string.IsNullOrWhiteSpace(empCode))
                {
                    result.FailedCount++;
                    result.Errors.Add(new ImportRowErrorDto { RowIndex = rowNum, Identifier = $"Row {rowNum}", ErrorMessage = "EmployeeCode is required for operational data import." });
                    continue;
                }

                try
                {
                    var empId = await connection.ExecuteScalarAsync<long?>(
                        "SELECT TOP 1 EmployeeID FROM hr.Employee WHERE TenantID = @TenantID AND EmployeeCode = @Code AND IsDeleted = 0",
                        new { TenantID = tenantId, Code = empCode });

                    if (!empId.HasValue)
                    {
                        result.FailedCount++;
                        result.Errors.Add(new ImportRowErrorDto { RowIndex = rowNum, Identifier = empCode, ErrorMessage = $"Employee {empCode} not found." });
                        continue;
                    }

                    if (entityType.ToLower() == "attendance")
                    {
                        string attDate = GetVal(row, "AttendanceDate", "Date") ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
                        string punchIn = GetVal(row, "PunchInTime", "PunchIn") ?? "09:00:00";
                        string punchOut = GetVal(row, "PunchOutTime", "PunchOut") ?? "18:00:00";
                        string status = GetVal(row, "Status") ?? "Present";

                        await connection.ExecuteAsync(
                            @"INSERT INTO attendance.Attendance (TenantID, EmployeeID, AttendanceDate, PunchInTime, PunchOutTime, TotalHoursWorked, [Status], CreatedBy, IsDeleted)
                              VALUES (@TenantID, @EmployeeID, @AttendanceDate, @PunchInTime, @PunchOutTime, 8.0, @Status, @UserId, 0);",
                            new { TenantID = tenantId, EmployeeID = empId.Value, AttendanceDate = attDate, PunchInTime = punchIn, PunchOutTime = punchOut, Status = status, UserId = userId });
                        result.SuccessCount++;
                    }
                    else
                    {
                        result.SuccessCount++;
                    }
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.Errors.Add(new ImportRowErrorDto { RowIndex = rowNum, Identifier = empCode, ErrorMessage = ex.Message });
                }
            }

            result.Message = $"Operational Data Import ({entityType}) Completed. Total: {result.TotalRows}, Success: {result.SuccessCount}, Failed: {result.FailedCount}";
            return result;
        }

        private List<Dictionary<string, string>> ReadCsvRows(Stream stream)
        {
            var list = new List<Dictionary<string, string>>();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            
            string headerLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(headerLine)) return list;

            var headers = ParseCsvLine(headerLine);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var values = ParseCsvLine(line);
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < headers.Length; i++)
                {
                    string val = i < values.Length ? values[i] : "";
                    dict[headers[i]] = val;
                }
                list.Add(dict);
            }
            return list;
        }

        private string[] ParseCsvLine(string line)
        {
            if (string.IsNullOrEmpty(line)) return Array.Empty<string>();

            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString().Trim('"', ' ', '\t', '\r', '\n'));
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            result.Add(sb.ToString().Trim('"', ' ', '\t', '\r', '\n'));

            return result.ToArray();
        }

        private string GetVal(Dictionary<string, string> dict, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (dict.TryGetValue(key, out var val) && !string.IsNullOrWhiteSpace(val))
                {
                    return val.Trim();
                }

                string normKey = NormalizeHeader(key);
                foreach (var kvp in dict)
                {
                    if (NormalizeHeader(kvp.Key) == normKey && !string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        return kvp.Value.Trim();
                    }
                }
            }
            return null;
        }

        private string NormalizeHeader(string header)
        {
            if (string.IsNullOrEmpty(header)) return string.Empty;
            return header.Replace(" ", "").Replace("_", "").Replace("-", "").Replace("/", "").Replace("\\", "").ToLowerInvariant();
        }

        private string SafeSubstring(string val, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(val)) return string.Empty;
            val = val.Trim();
            return val.Length <= maxLength ? val : val.Substring(0, maxLength);
        }
    }
}
