using Quartz;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Jobs
{
    [DisallowConcurrentExecution]
    public class TaskReminderJob(
        ITaskRepository repository,
        ISmsService smsService
    ) : IJob
    {
        public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var taskId = context.MergedJobDataMap.GetString("TaskId");
            if (string.IsNullOrEmpty(taskId)) return;

            var task = await repository.GetTaskById(Guid.Parse(taskId), CancellationToken.None);

            if (task == null || task.IsDeleted || task.IsCompleted) return;

            var message = $"In 5 minutes, the task {task.Title} is about to finish.";

            await smsService.SmsReminderAsync(task.UserPhoneNumber, message, CancellationToken.None);
        }
    }
}
