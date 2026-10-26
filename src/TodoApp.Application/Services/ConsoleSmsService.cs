using Microsoft.Extensions.Logging;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Services
{
    public class ConsoleSmsService(ILogger<ConsoleSmsService> logger) : ISmsService
    {
        public Task SmsReminderAsync(string phoneNumber, string message, CancellationToken cancellationToken)
        {
            logger.LogWarning("====== SMS SIMULADO ======");
            logger.LogWarning("Para: {Phone}", phoneNumber);
            logger.LogWarning("Mensagem: {Message}", message);
            logger.LogWarning("==========================");
            return Task.CompletedTask;
        }
    }
}
