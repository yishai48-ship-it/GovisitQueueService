using GovisitQueueService.Data;
using GovisitQueueService.DTOs;
using GovisitQueueService.Models;
using MediatR;
using MongoDB.Driver;

namespace GovisitQueueService.CQRS.Queries;

/// <summary>Returns every appointment, earliest first.</summary>
public record GetAppointmentsQuery : IRequest<List<AppointmentDto>>;

public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
{
    private readonly MongoDbContext _context;

    public GetAppointmentsQueryHandler(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _context.Appointments
            .Find(FilterDefinition<Appointment>.Empty)
            .SortBy(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);

        return appointments.Select(AppointmentMapper.ToDto).ToList();
    }
}
