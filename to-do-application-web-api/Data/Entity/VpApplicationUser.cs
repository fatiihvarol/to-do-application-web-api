using to_do_application_web_api.Base.BaseEntity;

namespace to_do_application_web_api.Data.Entity
{
    public class VpApplicationUser:VpBaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
