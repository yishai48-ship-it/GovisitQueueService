namespace GovisitQueueService.DTOs;

/// <summary>
/// Shared validation patterns. Kept in one place so the same rule is not
/// re-declared (and drifted) across DTOs.
/// </summary>
public static class ValidationPatterns
{
    // Hebrew (U+0590-U+05FF) and Latin letters, plus the separators that legitimately
    // appear in names: space, apostrophe, hyphen.
    // Anchored with \z rather than $, which in .NET also matches before a trailing newline.
    public const string PersonName = @"\A[A-Za-z\u0590-\u05FF]+([ '\-][A-Za-z\u0590-\u05FF]+){0,4}\z";

    // Israeli numbers: mobile 05X or landline area codes 2,3,4,8,9, optional single hyphen.
    public const string IsraeliPhone = @"\A0(?:5\d|[2-489])-?\d{7}\z";

    // Free-text service name, restricted to printable name characters.
    public const string ServiceType = @"\A[A-Za-z\u0590-\u05FF0-9]+([ '\-][A-Za-z\u0590-\u05FF0-9]+){0,9}\z";

    // The set of states an appointment is allowed to be in.
    public const string AppointmentStatus = @"\A(Scheduled|Completed|Cancelled)\z";

    // A MongoDB ObjectId rendered as a 24-character hex string.
    public const string ObjectId = @"\A[0-9a-fA-F]{24}\z";
}
