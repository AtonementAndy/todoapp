namespace TodoApp.Application.DTOs
{
    public record RegisterRequest(
        string Name,
        string Surname,
        string PhoneNumber,
        string Email,
        string Password
    );
}
