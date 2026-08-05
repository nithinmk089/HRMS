using HRMS.Application.Interfaces.Repositories;
using HRMS.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IBusinessUnitRepository, BusinessUnitRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ICostCenterRepository, CostCenterRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeEmploymentRepository, EmployeeEmploymentRepository>();
            services.AddScoped<IEmployeeAddressRepository, EmployeeAddressRepository>();
            services.AddScoped<IEmployeeContactRepository, EmployeeContactRepository>();
            services.AddScoped<IEmployeeEmergencyContactRepository, EmployeeEmergencyContactRepository>();
            services.AddScoped<IEmployeeQualificationRepository, EmployeeQualificationRepository>();
            services.AddScoped<IEmployeeCertificationRepository, EmployeeCertificationRepository>();
            services.AddScoped<IEmployeeDocumentRepository, EmployeeDocumentRepository>();
            services.AddScoped<IEmployeeManagerRepository, EmployeeManagerRepository>();
            services.AddScoped<IEmployeeTransferRepository, EmployeeTransferRepository>();
            services.AddScoped<IEmployeePromotionRepository, EmployeePromotionRepository>();
            services.AddScoped<IEmployeeStatusHistoryRepository, EmployeeStatusHistoryRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IOvertimeRepository, OvertimeRepository>();
            services.AddScoped<IOnboardingRepository, OnboardingRepository>();
            services.AddScoped<IOffboardingRepository, OffboardingRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<IPerformanceRepository, PerformanceRepository>();
            services.AddScoped<ILearningRepository, LearningRepository>();
            services.AddScoped<ITravelRepository, TravelRepository>();
            return services;

        }
    }
}
