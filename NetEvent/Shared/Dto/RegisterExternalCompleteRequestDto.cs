using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class RegisterExternalCompleteRequestDto
{
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = default!;
}
