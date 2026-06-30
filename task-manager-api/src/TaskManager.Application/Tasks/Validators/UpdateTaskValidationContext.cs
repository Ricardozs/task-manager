using TaskManager.Application.Tasks.Commands;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks.Validators;

public record UpdateTaskValidationContext(UpdateTaskCommand Command, TaskItem ExistingTask);
