namespace Wallet.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        protected Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() 
            => new Result(true, null);

        public static Result Failure(string errorMessage)
            => new Result(false, errorMessage);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, string? errorMessage, T? value)
            : base(isSuccess, errorMessage)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, null, value);
        
        public static new Result<T> Failure(string errorMessage)
            => new Result<T>(false, errorMessage, default);
    }
}
