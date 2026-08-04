using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Feedback.DTOs;

namespace backend.Modules.CustomerFeedback.Feedback.Interfaces;

public interface IFeedbackRepository
{
    Task<Entities.Feedback?> GetByIdAsync(Guid id, Guid organizationId);
    Task<(IEnumerable<Entities.Feedback> Items, int TotalCount)> SearchAsync(FeedbackSearchFilterDto filter);
    Task<IEnumerable<Entities.Feedback>> GetForExportAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<Entities.Feedback>> GetUnassignedUrgentAsync(int batchSize);
    Task<Entities.Feedback> AddAsync(Entities.Feedback entity);
    Task UpdateAsync(Entities.Feedback entity);
}

public interface IFeedbackService
{
    Task<FeedbackDto> SubmitFeedbackAsync(SubmitFeedbackRequestDto request);
    Task<FeedbackDto?> GetByIdAsync(Guid id, Guid organizationId);
    Task<FeedbackPagedResultDto> SearchAsync(FeedbackSearchFilterDto filter);
    Task<string> ExportToCsvAsync(Guid organizationId, DateTime? startDate, DateTime? endDate);
    Task<FeedbackDto?> UpdateStatusAsync(Guid id, Guid organizationId, string status, string? resolutionNotes);
}
