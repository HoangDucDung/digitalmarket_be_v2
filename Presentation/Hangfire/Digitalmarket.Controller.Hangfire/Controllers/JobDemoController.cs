using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Digitalmarket.Controller.Hangfire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobDemoController(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, ILogger<JobDemoController> logger) 
        : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;
        private readonly ILogger<JobDemoController> _logger = logger;

        /// <summary>
        /// Tạo một Job chạy ngay lập tức (Fire-and-forget)
        /// </summary>
        [HttpPost("enqueue")]
        public IActionResult EnqueueJob(string message)
        {
            var jobId = _backgroundJobClient.Enqueue(() => ProcessMessage(message));
            return Ok(new { JobId = jobId, Message = "Job đã được đưa vào hàng đợi." });
        }

        /// <summary>
        /// Lên lịch chạy Job sau một khoảng thời gian (Delayed)
        /// </summary>
        [HttpPost("schedule")]
        public IActionResult ScheduleJob(string message, int delaySeconds)
        {
            var jobId = _backgroundJobClient.Schedule(() => ProcessMessage(message), TimeSpan.FromSeconds(delaySeconds));
            return Ok(new { JobId = jobId, Message = $"Job đã được lên lịch chạy sau {delaySeconds} giây." });
        }

        /// <summary>
        /// Tạo hoặc cập nhật một Job chạy định kỳ (Recurring)
        /// </summary>
        /// <param name="message">Thông điệp cần log</param>
        /// <param name="cronExpression">Cron expression (VD: "*/1 * * * *" là mỗi phút)</param>
        [HttpPost("recurring")]
        public IActionResult RecurringJob(string message, string cronExpression = "*/1 * * * *")
        {
            _recurringJobManager.AddOrUpdate("demo-recurring-job", () => ProcessMessage(message), cronExpression);
            return Ok(new { Message = $"Job định kỳ đã được thiết lập với Cron: {cronExpression}" });
        }

        /// <summary>
        /// Xóa một Job định kỳ
        /// </summary>
        [HttpDelete("remove-recurring")]
        public IActionResult RemoveRecurringJob(string jobId = "demo-recurring-job")
        {
            _recurringJobManager.RemoveIfExists(jobId);
            return Ok(new { Message = $"Job {jobId} đã được xóa." });
        }

        // Phương thức này sẽ được Hangfire gọi. 
        // Lưu ý: Phải là public để Hangfire có thể truy cập được từ bên ngoài.
        [NonAction] // Tránh để Swagger coi đây là một endpoint
        public void ProcessMessage(string message)
        {
            _logger.LogInformation($"[HANGFIRE JOB EXECUTION] Time: {DateTime.Now:HH:mm:ss} | Message: {message}");
        }
    }
}
