namespace BlazorMud.Contracts.Services
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ServiceResult(bool isSuccess = true, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }

    public sealed class ServiceResult<TResult> : ServiceResult
    {
        public TResult Result { get; set; }

        public ServiceResult(bool isSuccess = true, string message = "", TResult result = default)
            : base(isSuccess, message)
        {
            Result = result;
        }
    }
}
