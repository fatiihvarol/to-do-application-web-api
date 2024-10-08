using to_do_application_web_api.Base.BaseEntity;
using to_do_application_web_api.Base.Enum;

namespace to_do_application_web_api.Data.Entity
{
    public class VpTodoItem:VpBaseEntity
    {
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public string? Detail { get; set; }
        public PriorityEnum Priority { get; set; }
        public int UserId { get; set; }
        public VpUser? User { get; set; }
    }
}
