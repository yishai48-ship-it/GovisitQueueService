using GovisitQueueService.Data;
using GovisitQueueService.DTOs;
using GovisitQueueService.Models;
using MediatR;
using MongoDB.Driver;

namespace GovisitQueueService.CQRS.Queries;

/// <summary>Returns a single appointment, or null when the id does not exist.</summary>
public record GetAppointmentByIdQuery(string Id) : IRequest<AppointmentDto?>;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly MongoDbContext _context;

    public GetAppointmentByIdQueryHandler(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Appointment>.Filter.Eq(a => a.Id, request.Id);

        var appointment = await _context.Appointments.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return appointment is null ? null : AppointmentMapper.ToDto(appointment);
    }
}
