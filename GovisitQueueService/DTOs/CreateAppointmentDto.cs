using System.ComponentModel.DataAnnotations;

namespace GovisitQueueService.DTOs;

public class CreateAppointmentDto
{
    [Required]
    [RegularExpression(ValidationPatterns.PersonName,
        ErrorMessage = "Customer name must be 2-50 Hebrew or Latin letters, spaces, apostrophes or hyphens.")]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(ValidationPatterns.IsraeliPhone,
        ErrorMessage = "Phone must be a valid Israeli number, e.g. 0521234567 or 052-1234567.")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [RegularExpression(ValidationPatterns.ServiceType,
        ErrorMessage = "Service type must be 2-60 letters, digits, spaces, apostrophes or hyphens.")]
    public string ServiceType { get; set; } = string.Empty;

    [Required]
    public DateTime AppointmentDate { get; set; }
}
