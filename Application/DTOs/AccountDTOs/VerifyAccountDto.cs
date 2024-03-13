namespace Application.DTOs.AccountDTOs;

public class VerifyAccountDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string VerificationToken { get; set; }
}