namespace Wallet.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyCollection<string> ErrorMessages { get; }

        protected Result(
            bool isSuccess, 
            IReadOnlyCollection<string>? errorMessages = null)
        {
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages ?? [];
        }

        public static Result Success() 
            => new Result(true);

        public static Result Failure(params string[] errorMessages)
            => new Result(false, errorMessages);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(
            bool isSuccess,
            T? value,
            IReadOnlyCollection<string>? errorMessages = null)
            : base(isSuccess, errorMessages)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, value);
        
        public static new Result<T> Failure(params string[] errorMessages)
            => new Result<T>(false, default, errorMessages);
    }
}
