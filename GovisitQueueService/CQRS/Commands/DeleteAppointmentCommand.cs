using GovisitQueueService.Data;
using GovisitQueueService.Models;
using MediatR;
using MongoDB.Driver;

namespace GovisitQueueService.CQRS.Commands;

/// <summary>Cancels an appointment by removing it. False means no document matched the id.</summary>
public record DeleteAppointmentCommand(string Id) : IRequest<bool>;

public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, bool>
{
    private readonly MongoDbContext _context;

    public DeleteAppointmentCommandHandler(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<Appointment>.Filter.Eq(a => a.Id, request.Id);

        var result = await _context.Appointments.DeleteOneAsync(filter, cancellationToken);

        return result.DeletedCount > 0;
    }
}
