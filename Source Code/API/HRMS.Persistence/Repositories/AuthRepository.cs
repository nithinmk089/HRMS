using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs.Auth;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SetupStatusResponse> GetSetupStatusAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var userCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM security.[User] WHERE IsDeleted = 0");
            
            var tenantCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM security.Tenant WHERE IsDeleted = 0");

            bool needsSetup = userCount == 0 || tenantCount == 0;

            return new SetupStatusResponse
            {
                NeedsSetup = needsSetup,
                ExistingTenantsCount = tenantCount,
                ExistingUsersCount = userCount,
                SystemMessage = needsSetup 
                    ? "Fresh installation detected. First-time system administration setup required."
                    : "System initialization completed."
            };
        }

        public async Task<AuthResponse> PerformInitialSetupAsync(InitialSystemSetupRequest request, string ipAddress, string browserInfo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Create Tenant (TenantCode must be alphanumeric and >= 3 chars per trigger trg_Tenant_Code_Validation)
                string tenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? "TEN-ACME-001" : request.TenantCode;
                string tenantName = string.IsNullOrWhiteSpace(request.TenantName) ? "Acme Enterprise Group" : request.TenantName;

                var tenantId = await connection.ExecuteScalarAsync<long>(
                    @"INSERT INTO security.Tenant (TenantCode, TenantName, [Status], EffectiveFrom, CreatedBy, IsDeleted, VersionNo)
                      VALUES (@TenantCode, @TenantName, 'Active', GETUTCDATE(), 1, 0, 1);
                      SELECT SCOPE_IDENTITY();",
                    new { TenantCode = tenantCode, TenantName = tenantName },
                    transaction
                );

                // 2. Create Company
                string companyCode = string.IsNullOrWhiteSpace(request.CompanyCode) ? "COMP-ACME-TECH" : request.CompanyCode;
                string companyName = string.IsNullOrWhiteSpace(request.CompanyName) ? "Acme Technology Corp" : request.CompanyName;

                var companyId = await connection.ExecuteScalarAsync<long>(
                    @"INSERT INTO security.Company (TenantID, CompanyCode, CompanyName, LegalName, TaxNumber, Email, Phone, CreatedBy, IsDeleted, VersionNo)
                      VALUES (@TenantID, @CompanyCode, @CompanyName, @LegalName, @TaxNumber, @Email, @Phone, 1, 0, 1);
                      SELECT SCOPE_IDENTITY();",
                    new { 
                        TenantID = tenantId, 
                        CompanyCode = companyCode, 
                        CompanyName = companyName,
                        LegalName = request.LegalName ?? companyName,
                        TaxNumber = request.TaxNumber ?? "TAX-001",
                        Email = request.CompanyEmail ?? request.AdminEmail,
                        Phone = request.CompanyPhone ?? request.MobileNumber ?? "555-0100"
                    },
                    transaction
                );

                // 3. Provision System Configurations for new Tenant
                await connection.ExecuteAsync(
                    @"INSERT INTO system.Configuration (TenantID, ConfigurationKey, ConfigurationValue, DataType, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      VALUES 
                      (@TenantID, 'PasswordMinLength', '12', 'Int', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PasswordRequireComplexity', 'True', 'Boolean', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'SessionTimeoutMinutes', '30', 'Int', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'MaxFailedLoginAttempts', '5', 'Int', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'DefaultCurrency', 'USD', 'String', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'AttendanceGraceMinutes', '15', 'Int', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'LeaveMaxCarryForwardDays', '10', 'Int', 1, GETUTCDATE(), 0, 1);",
                    new { TenantID = tenantId },
                    transaction
                );

                // 4. Provision Master System Roles for new Tenant
                await connection.ExecuteAsync(
                    @"INSERT INTO security.[Role] (TenantID, RoleCode, RoleName, [Description], CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      VALUES 
                      (@TenantID, 'SYSADMIN', 'System Administrator', 'Full platform administrative control across all modules', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'HRADMIN', 'HR Administrator', 'Core HR, employee lifecycle, performance, LMS, and asset management', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PAYROLLADMIN', 'Payroll & Tax Administrator', 'Payroll processing, tax regimes, and statutory deduction management', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'MANAGER', 'Line Manager', 'Team management, leave/expense approvals, and performance reviews', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ESS', 'Employee Self Service', 'General employee self-service features, leave apply, and LMS learning', 1, GETUTCDATE(), 0, 1);",
                    new { TenantID = tenantId },
                    transaction
                );

                // 5. Provision Master System Permissions for new Tenant
                await connection.ExecuteAsync(
                    @"INSERT INTO security.[Permission] (TenantID, PermissionCode, PermissionName, ModuleCode, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      VALUES 
                      (@TenantID, 'SYSADMIN', 'Full Platform Administration', 'SECURITY', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'USER_MANAGE', 'User Account Management', 'SECURITY', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ROLE_MANAGE', 'Security Role Management', 'SECURITY', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PERM_MANAGE', 'Permission Matrix Management', 'SECURITY', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'TENANT_MANAGE', 'Tenant Administration', 'SECURITY', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'CONF_MANAGE', 'System Configurations', 'SYSTEM', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'AUDIT_VIEW', 'Audit Log Viewing', 'SYSTEM', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ORG_MANAGE', 'Organization Structure Management', 'ORGANIZATION', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ORG_VIEW', 'View Organization Structure', 'ORGANIZATION', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'EMP_MANAGE', 'Employee Master Management', 'HR', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'EMP_VIEW', 'View Employee Directory & Profiles', 'HR', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'TRANSFER_MANAGE', 'Employee Transfers Workflow', 'HR', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PROMOTION_MANAGE', 'Employee Promotions Workflow', 'HR', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ONBOARD_MANAGE', 'Onboarding Dashboard & Tasks', 'ONBOARDING', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'OFFBOARD_MANAGE', 'Offboarding & Exit Workflow', 'OFFBOARDING', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ATT_MANAGE', 'Shifts & Attendance Management', 'ATTENDANCE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ATT_VIEW', 'View Attendance Logs', 'ATTENDANCE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'LEAVE_MANAGE', 'Leave Policies & Balances', 'LEAVE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'LEAVE_APPLY', 'Apply for Leave', 'LEAVE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'LEAVE_APPROVE', 'Approve Leave Applications', 'LEAVE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'OVERTIME_MANAGE', 'Overtime Requests & Approvals', 'ATTENDANCE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ASSET_MANAGE', 'Asset Register & Categories', 'ASSET', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ASSET_ASSIGN', 'Asset Assignment & Return', 'ASSET', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'ASSET_VIEW', 'View Asset Stock', 'ASSET', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PAYROLL_MANAGE', 'Payroll Processing & Salary Components', 'PAYROLL', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PAYROLL_VIEW', 'View Payroll Runs & Payslips', 'PAYROLL', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'TAX_MANAGE', 'Tax Regimes, Slabs & Declarations', 'TAX', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PERF_MANAGE', 'PMS Appraisals & Goal Configuration', 'PERFORMANCE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'PERF_VIEW', 'View Appraisals & Ratings', 'PERFORMANCE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'GOAL_MANAGE', 'Individual & Team Goal Setting', 'PERFORMANCE', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'LMS_MANAGE', 'LMS Course Catalog & Programs', 'LEARNING', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'LMS_VIEW', 'View LMS Catalog & Courses', 'LEARNING', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'TRAVEL_MANAGE', 'Travel Requests & Approvals', 'TRAVEL', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'EXPENSE_MANAGE', 'Expense Claims & Reimbursements', 'TRAVEL', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'CARD_MANAGE', 'Corporate Card Reconciliation', 'TRAVEL', 1, GETUTCDATE(), 0, 1),
                      (@TenantID, 'NOTIF_MANAGE', 'Notification Templates & Preferences', 'COMMUNICATION', 1, GETUTCDATE(), 0, 1);",
                    new { TenantID = tenantId },
                    transaction
                );

                // 6. Provision RolePermission Mappings for new Tenant
                await connection.ExecuteAsync(
                    @"-- SYSADMIN -> ALL
                      INSERT INTO security.RolePermission (TenantID, RoleID, PermissionID, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, r.RoleID, p.PermissionID, 1, GETUTCDATE(), 0, 1
                      FROM security.[Role] r
                      CROSS JOIN security.[Permission] p
                      WHERE r.TenantID = @TenantID AND r.RoleCode = 'SYSADMIN' AND p.TenantID = @TenantID;

                      -- HRADMIN
                      INSERT INTO security.RolePermission (TenantID, RoleID, PermissionID, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, r.RoleID, p.PermissionID, 1, GETUTCDATE(), 0, 1
                      FROM security.[Role] r
                      INNER JOIN security.[Permission] p ON p.TenantID = @TenantID AND p.ModuleCode IN ('HR', 'ORGANIZATION', 'ONBOARDING', 'OFFBOARDING', 'ATTENDANCE', 'LEAVE', 'ASSET', 'PERFORMANCE', 'LEARNING', 'COMMUNICATION')
                      WHERE r.TenantID = @TenantID AND r.RoleCode = 'HRADMIN';

                      -- PAYROLLADMIN
                      INSERT INTO security.RolePermission (TenantID, RoleID, PermissionID, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, r.RoleID, p.PermissionID, 1, GETUTCDATE(), 0, 1
                      FROM security.[Role] r
                      INNER JOIN security.[Permission] p ON p.TenantID = @TenantID AND (p.ModuleCode IN ('PAYROLL', 'TAX') OR p.PermissionCode IN ('ATT_VIEW', 'EMP_VIEW'))
                      WHERE r.TenantID = @TenantID AND r.RoleCode = 'PAYROLLADMIN';

                      -- MANAGER
                      INSERT INTO security.RolePermission (TenantID, RoleID, PermissionID, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, r.RoleID, p.PermissionID, 1, GETUTCDATE(), 0, 1
                      FROM security.[Role] r
                      INNER JOIN security.[Permission] p ON p.TenantID = @TenantID AND p.PermissionCode IN ('EMP_VIEW', 'LEAVE_APPROVE', 'ATT_VIEW', 'OVERTIME_MANAGE', 'TRAVEL_MANAGE', 'EXPENSE_MANAGE', 'PERF_MANAGE', 'GOAL_MANAGE')
                      WHERE r.TenantID = @TenantID AND r.RoleCode = 'MANAGER';

                      -- ESS
                      INSERT INTO security.RolePermission (TenantID, RoleID, PermissionID, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, r.RoleID, p.PermissionID, 1, GETUTCDATE(), 0, 1
                      FROM security.[Role] r
                      INNER JOIN security.[Permission] p ON p.TenantID = @TenantID AND p.PermissionCode IN ('EMP_VIEW', 'ORG_VIEW', 'LEAVE_APPLY', 'ATT_VIEW', 'ASSET_VIEW', 'PAYROLL_VIEW', 'PERF_VIEW', 'GOAL_MANAGE', 'LMS_VIEW', 'EXPENSE_MANAGE', 'TRAVEL_MANAGE')
                      WHERE r.TenantID = @TenantID AND r.RoleCode = 'ESS';",
                    new { TenantID = tenantId },
                    transaction
                );

                // 7. Create Admin Employee Record
                string empCode = string.IsNullOrWhiteSpace(request.EmployeeCode) ? "EMP-001" : request.EmployeeCode;
                string firstName = string.IsNullOrWhiteSpace(request.FirstName) ? "Admin" : request.FirstName;
                string lastName = string.IsNullOrWhiteSpace(request.LastName) ? "User" : request.LastName;

                var employeeId = await connection.ExecuteScalarAsync<long>(
                    @"INSERT INTO hr.Employee (TenantID, EmployeeCode, EmployeeNumber, FirstName, LastName, PersonalEmail, MobileNumber, [Status], CreatedBy, IsDeleted)
                      VALUES (@TenantID, @EmployeeCode, @EmployeeNumber, @FirstName, @LastName, @PersonalEmail, @MobileNumber, 'Active', 1, 0);
                      SELECT SCOPE_IDENTITY();",
                    new {
                        TenantID = tenantId,
                        EmployeeCode = empCode,
                        EmployeeNumber = empCode,
                        FirstName = firstName,
                        LastName = lastName,
                        PersonalEmail = request.AdminEmail,
                        MobileNumber = request.MobileNumber ?? "555-0100"
                    },
                    transaction
                );

                // 8. Create Admin User Account in security.[User]
                string passwordHash = "AQAAAAEAACcQAAAAEHASH"; // Standard hash placeholder
                string passwordSalt = "SALT123";

                var userId = await connection.ExecuteScalarAsync<long>(
                    @"INSERT INTO security.[User] (TenantID, EmployeeID, UserName, Email, PasswordHash, PasswordSalt, IsLocked, CreatedBy, IsDeleted, VersionNo)
                      VALUES (@TenantID, @EmployeeID, @UserName, @Email, @PasswordHash, @PasswordSalt, 0, 1, 0, 1);
                      SELECT SCOPE_IDENTITY();",
                    new {
                        TenantID = tenantId,
                        EmployeeID = employeeId,
                        UserName = request.AdminEmail,
                        Email = request.AdminEmail,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    },
                    transaction
                );

                // 9. Assign SYSADMIN & HRADMIN roles to Admin User
                await connection.ExecuteAsync(
                    @"INSERT INTO security.UserRole (TenantID, UserID, RoleID, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, @UserID, RoleID, 1, GETUTCDATE(), 0, 1
                      FROM security.[Role]
                      WHERE TenantID = @TenantID AND RoleCode IN ('SYSADMIN', 'HRADMIN');",
                    new { TenantID = tenantId, UserID = userId },
                    transaction
                );

                // 10. Assign Direct User Permissions to Admin User
                await connection.ExecuteAsync(
                    @"INSERT INTO security.UserPermission (TenantID, UserID, PermissionID, IsAllowed, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                      SELECT @TenantID, @UserID, PermissionID, 1, 1, GETUTCDATE(), 0, 1
                      FROM security.[Permission]
                      WHERE TenantID = @TenantID;",
                    new { TenantID = tenantId, UserID = userId },
                    transaction
                );

                transaction.Commit();

                // Login automatically after setup
                return await LoginAsync(request.AdminEmail, request.Password, ipAddress, browserInfo);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new AuthResponse { Success = false, Message = "Initial system setup failed: " + ex.Message };
            }
        }

        public async Task<AuthResponse> LoginAsync(string email, string password, string ipAddress, string browserInfo)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            parameters.Add("@IPAddress", ipAddress);
            parameters.Add("@BrowserInfo", browserInfo);

            using var multi = await connection.QueryMultipleAsync("security.usp_User_Login", parameters, commandType: CommandType.StoredProcedure);
            
            var firstResult = await multi.ReadFirstOrDefaultAsync();
            if (firstResult != null && ((IDictionary<string, object>)firstResult).ContainsKey("Success"))
            {
                bool success = Convert.ToBoolean(firstResult.Success);
                if (!success)
                {
                    return new AuthResponse { Success = false, Message = firstResult.Message };
                }
            }

            var userRow = firstResult; 

            var userDto = new UserDto
            {
                UserId = userRow != null ? Convert.ToInt64(userRow.UserID) : 1,
                Email = userRow != null ? Convert.ToString(userRow.Email) : email,
                FirstName = userRow != null ? Convert.ToString(userRow.FirstName) : "Admin",
                LastName = userRow != null ? Convert.ToString(userRow.LastName) : "User",
                OrganizationId = userRow != null ? Convert.ToInt64(userRow.OrganizationID) : 1,
                TenantId = userRow != null ? Convert.ToInt64(userRow.TenantID) : 1
            };

            var roles = (await multi.ReadAsync<string>()).ToList();
            var permissions = (await multi.ReadAsync<string>()).ToList();
            var companies = (await multi.ReadAsync<CompanyDto>()).ToList();

            return new AuthResponse
            {
                Success = true,
                User = userDto,
                Roles = roles,
                Permissions = permissions,
                Companies = companies
            };
        }
    }
}
