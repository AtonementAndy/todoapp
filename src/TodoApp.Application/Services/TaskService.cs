using Quartz;
using TodoApp.Application.Common;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Jobs;
using TodoApp.Application.Models.Entities;

namespace TodoApp.Application.Services
{
    public class TaskService(
        ITaskRepository repository,
        ISchedulerFactory schedulerFactory
    ) : ITaskService
    {
        public async Task<Result<TaskDto>> AddAsync(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var newTask = TodoTask.Create(
                request.Title,
                request.Description,
                request.UserId,
                request.DueDate
            );

            var taskDto = await repository.AddAsync(newTask, cancellationToken);

            if (newTask.DueDate.HasValue)
            {
                var reminderTime = newTask.DueDate.Value.AddMinutes(-5);

                if (reminderTime > DateTime.UtcNow)
                {
                    var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
                    var jobKey = new JobKey($"TaskReminder_{newTask.Id}");
                    var jobDetail = JobBuilder.Create<TaskReminderJob>()
                        .WithIdentity(jobKey)
                        .UsingJobData("TaskId", newTask.Id.ToString())
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .StartAt(new DateTimeOffset(reminderTime))
                        .Build();

                    await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken: cancellationToken);
                }
            }

            return Result<TaskDto>.Success(taskDto);
        }
    }
}
