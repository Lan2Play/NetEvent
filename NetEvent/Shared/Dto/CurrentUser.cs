using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class CurrentUser : User
{
    [JsonPropertyName("isauthenticated")]
    public bool IsAuthenticated { get; set; }

    [JsonPropertyName("claims")]
    public Dictionary<string, string> Claims { get; set; }

}
