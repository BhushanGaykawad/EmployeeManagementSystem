namespace EmployeeManagementSystem.CustomizedExceptions
{
    public class CustomizedException:Exception
    {
        public CustomizedException(string message):base(message) { }

        public CustomizedException(string message,Exception insideException):base(message,insideException) { }
    }
}
