namespace to_do_application_web_api.Data.Entity
{
    public class VpUser:VpApplicationUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public List<VpTodoItem>? VpTodoItems { get; set; }
    }
}
