using System;
using System.Collections.Generic;
using System.Text;

namespace Wallet.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        private Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() 
            => new Result(true, null);

        public static Result Failure(string errorMessage)
            => new Result(false, errorMessage);
    }
}
