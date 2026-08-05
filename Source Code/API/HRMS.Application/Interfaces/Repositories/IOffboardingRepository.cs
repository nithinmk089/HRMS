using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IOffboardingRepository
    {
        Task<long> CreateExitRequestAsync(CreateExitRequest request);
        Task<bool> UpdateExitRequestAsync(UpdateExitRequest request);
        Task<bool> SubmitExitRequestAsync(long requestId, long tenantId, long userId);
        Task<long> ApproveExitRequestAsync(ApproveExitRequest request);
        Task<long> RejectExitRequestAsync(RejectExitRequest request);

        Task<long> CreateClearanceRequestAsync(CreateClearanceRequest request);
        Task<bool> ApproveClearanceRequestAsync(long clearanceRequestId, long tenantId, long userId);
        Task<bool> CompleteClearanceTaskAsync(long clearanceTaskId, long tenantId, long userId);

        Task<long> CreateAssetReturnAsync(CreateAssetReturnRequest request);
        Task<bool> VerifyAssetReturnAsync(VerifyAssetReturnRequest request);

        Task<long> CreateKnowledgeTransferAsync(CreateKnowledgeTransferRequest request);
        Task<bool> CompleteKnowledgeTransferAsync(long ktId, long tenantId, long userId);

        Task<long> GenerateExperienceLetterAsync(GenerateExperienceLetterRequest request);
        Task<long> CalculateFullAndFinalSettlementAsync(CalculateFFSRequest request);
        Task<bool> ApproveFullAndFinalSettlementAsync(long ffsId, long tenantId, long userId);
        Task<bool> CloseFullAndFinalSettlementAsync(long ffsId, long tenantId, long userId);

        Task<IEnumerable<ExitStatusReportDto>> GetExitStatusReportAsync(long tenantId);
        Task<IEnumerable<ClearanceStatusReportDto>> GetClearanceStatusReportAsync(long tenantId);
        Task<IEnumerable<FullAndFinalSummaryReportDto>> GetFullAndFinalSummaryReportAsync(long tenantId);
    }
}
