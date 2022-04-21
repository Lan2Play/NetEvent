using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class User
{
    [JsonPropertyName("username")]
    public string UserName { get; set; }



    [JsonPropertyName("id")]
    public string Id { get; }

    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastname")]
    public string LastName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("profileimage")]
    public IReadOnlyList<byte>? ProfileImage { get; set; }
}
