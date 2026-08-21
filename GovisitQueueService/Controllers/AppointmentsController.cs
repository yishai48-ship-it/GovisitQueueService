using System.ComponentModel.DataAnnotations;
using GovisitQueueService.CQRS.Commands;
using GovisitQueueService.CQRS.Queries;
using GovisitQueueService.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GovisitQueueService.Controllers;

/// <summary>
/// Queue management endpoints. The controller only translates HTTP to a command or
/// query; all behaviour lives in the MediatR handlers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Books a new appointment.</summary>
    [HttpPost("create")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create(
        [FromBody] CreateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var newId = await _mediator.Send(new CreateAppointmentCommand(dto), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
    }

    /// <summary>Lists all appointments, earliest first.</summary>
    [HttpPost("all")]
    [ProducesResponseType(typeof(List<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AppointmentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAppointmentsQuery(), cancellationToken);

        return Ok(result);
    }

    /// <summary>Returns a single appointment by identifier.</summary>
    [HttpPost("{id:length(24)}/details")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetById(
        [FromRoute][RegularExpression(ValidationPatterns.ObjectId)] string id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAppointmentByIdQuery(id), cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Updates an existing appointment.</summary>
    [HttpPost("{id:length(24)}/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute][RegularExpression(ValidationPatterns.ObjectId)] string id,
        [FromBody] UpdateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateAppointmentCommand(id, dto), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    /// <summary>Cancels an appointment.</summary>
    [HttpPost("{id:length(24)}/delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute][RegularExpression(ValidationPatterns.ObjectId)] string id,
        CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteAppointmentCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
