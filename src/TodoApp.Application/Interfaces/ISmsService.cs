namespace TodoApp.Application.Interfaces
{
    public interface ISmsService
    {
        Task SmsReminderAsync(string phoneNumber, string message, CancellationToken cancellationToken);
    }
}
