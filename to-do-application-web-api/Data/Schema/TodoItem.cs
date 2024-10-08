using to_do_application_web_api.Base.Enum;
using to_do_application_web_api.Data.Entity;

namespace to_do_application_web_api.Data.Schema
{
    public class TodoItemRequest
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public PriorityEnum Priority { get; set; }
    }
    public class TodoItemResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public string? Detail { get; set; }
        public string? Priority { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
