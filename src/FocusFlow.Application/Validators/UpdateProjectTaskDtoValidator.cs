using FluentValidation;

namespace FocusFlow.Application.Validators;

public class UpdateProjectTaskDtoValidator : AbstractValidator<UpdateProjectTaskDto>
{
    public UpdateProjectTaskDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.AssignedToId)
            .GreaterThan(0).WithMessage("Assigned To ID must be greater than 0")
            .When(x => x.AssignedToId.HasValue);

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value")
            .When(x => x.Priority.HasValue);
    }
}
