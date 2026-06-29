using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Commands;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController(
    ListTasksHandler listTasksHandler,
    CreateTaskHandler createTaskHandler,
    GetTaskHandler getTaskHandler,
    UpdateTaskHandler updateTaskHandler,
    DeleteTaskHandler deleteTaskHandler) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var query = new ListTasksQuery(userId);
        var tasks = await listTasksHandler.HandleAsync(query, cancellationToken);
        return Ok(tasks);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var command = new CreateTaskCommand(
            userId,
            request.Title,
            request.Description,
            request.Status,
            request.DueDate);

        var task = await createTaskHandler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var query = new GetTaskQuery(userId, id);
        var task = await getTaskHandler.HandleAsync(query, cancellationToken);
        return Ok(task);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var command = new UpdateTaskCommand(
            userId,
            id,
            request.Title,
            request.Description,
            request.Status,
            request.DueDate);

        var task = await updateTaskHandler.HandleAsync(command, cancellationToken);
        return Ok(task);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteTaskCommand(userId, id);
        await deleteTaskHandler.HandleAsync(command, cancellationToken);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User id claim is missing or invalid.");

        return userId;
    }
}
