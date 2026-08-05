using System;
using FluentValidation.TestHelper;
using HRMS.Application.DTOs;
using HRMS.Application.Validators;
using Xunit;

namespace HRMS.UnitTests.Validators
{
    public class EmployeeValidatorsTests
    {
        private readonly CreateEmployeeRequestValidator _createValidator;
        private readonly UpdateEmployeeRequestValidator _updateValidator;

        public EmployeeValidatorsTests()
        {
            _createValidator = new CreateEmployeeRequestValidator();
            _updateValidator = new UpdateEmployeeRequestValidator();
        }

        [Fact]
        public void CreateEmployee_ShouldHaveError_WhenFirstNameIsEmpty()
        {
            var request = new CreateEmployeeRequest { FirstName = "" };
            var result = _createValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void CreateEmployee_ShouldHaveError_WhenLastNameIsEmpty()
        {
            var request = new CreateEmployeeRequest { LastName = "" };
            var result = _createValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void CreateEmployee_ShouldHaveError_WhenEmployeeCodeIsNonAlphanumeric()
        {
            var request = new CreateEmployeeRequest { EmployeeCode = "EMP_123!" };
            var result = _createValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.EmployeeCode);
        }

        [Fact]
        public void CreateEmployee_ShouldHaveError_WhenEmployeeIsUnder15()
        {
            var request = new CreateEmployeeRequest { DateOfBirth = DateTime.UtcNow.AddYears(-10) };
            var result = _createValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void UpdateEmployee_ShouldHaveError_WhenEmployeeIdIsZero()
        {
            var request = new UpdateEmployeeRequest { EmployeeId = 0 };
            var result = _updateValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.EmployeeId);
        }
    }
}
