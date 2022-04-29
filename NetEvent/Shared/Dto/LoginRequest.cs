using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class LoginRequest
{
    [Required(ErrorMessage = "UserName is required.")]
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("rememberme")]
    public bool RememberMe { get; set; } = true;
}
