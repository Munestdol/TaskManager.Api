using FluentValidation;
using TaskManager.Application.Tasks.DTOs;

namespace TaskManager.Application.Tasks.Validators
{
    public sealed class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must be less than 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must be less than 2000 characters");

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 2).WithMessage("Status must be between 0 and 2");
        }
    }
}
