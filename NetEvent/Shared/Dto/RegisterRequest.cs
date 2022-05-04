using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetEvent.Shared.Dto;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Firstname is required.")]
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Lastname is required.")]
    [JsonPropertyName("lastname")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [JsonPropertyName("confirmpassword")]
    public string ConfirmPassword { get; set; }
}
