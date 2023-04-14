namespace HealthEquity.API.Models
{
    public class Result<T>
    {
        public bool Succeeded { get; }
        public T Value { get; }
        public string ErrorMessage { get; }

        private Result(bool succeeded, T value, string errorMessage)
        {
            Succeeded = succeeded;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(true, value, string.Empty);
        }

        public static Result<T> NotFound(string errorMessage)
        {
            return new Result<T>(false, default, errorMessage);
        }

        public static Result<T> Error(string errorMessage)
        {
            return new Result<T>(false, default, errorMessage);
        }
    }
}
