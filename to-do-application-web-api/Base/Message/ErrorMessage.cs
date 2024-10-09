using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace to_do_application_web_api.Base.Message
{
    public static class ErrorMessage
    {
        public interface TokenErrorMessage
        {
            public static string UserIdNotFound = "User Id not found in token";
            public static string EmptyModelError = "Model can not be empty";
            public static string UserNotFound = "User not found";

        }
        public interface TodoItemErrorMessage
        {
            public static string ModelNotFoundError = "Model not found on the request";
            public static string EmptyModelError = "Model can not be empty";
            public static string UserNotFound = "User not found";
            public static string TodoItemNotFound = "Item not found";
            public static string NotAuthorized = "You are not authorized update this item";

        }
    }
}
