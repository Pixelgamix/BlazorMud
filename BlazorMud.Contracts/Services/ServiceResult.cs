namespace BlazorMud.Contracts.Services
{
    /// <summary>
    /// Result of a service call.
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// <c>true</c> if the call was succesfull, <c>false</c> if an error occured.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Message for display to the user.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new <see cref="ServiceResult"/> instance with the specified parameters.
        /// </summary>
        /// <param name="isSuccess">Call success.</param>
        /// <param name="message">Message to the user.</param>
        public ServiceResult(bool isSuccess = true, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }

    /// <summary>
    /// Result of a service call with service call return data.
    /// </summary>
    /// <typeparam name="TResult">The type of the result the service returns.</typeparam>
    public sealed class ServiceResult<TResult> : ServiceResult
    {
        /// <summary>
        /// The service call's return data.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Creates a new <see cref="ServiceResult{TResult}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="isSuccess">Call success.</param>
        /// <param name="message">Message to the user.</param>
        /// <param name="result">The service call return data.</param>
        public ServiceResult(bool isSuccess = true, string message = "", TResult result = default)
            : base(isSuccess, message)
        {
            Result = result;
        }
    }
}
