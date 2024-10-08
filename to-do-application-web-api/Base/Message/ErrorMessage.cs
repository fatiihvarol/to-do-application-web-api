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

        }
    }
}
