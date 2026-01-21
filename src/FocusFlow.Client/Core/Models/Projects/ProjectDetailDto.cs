namespace FocusFlow.Client.Core.Models.Projects;

public record ProjectDetailDto : ProjectSimpleDto
{
    public string? Description { get; init; }
    public long OwnerId { get; init; }
    public string OwnerName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
