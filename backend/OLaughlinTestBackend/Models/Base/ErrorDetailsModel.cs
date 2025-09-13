namespace Models.Base
{
    public enum HttpStatusCodeEnum
    {
        Moved = 301,
        OK = 200,
        /// <summary>
        /// Some data couldn't be retrieved
        /// </summary>
        PartialContent = 206,
        /// <summary>
        /// Redirected
        /// </summary>
        Redirect = 302,
        /// <summary>
        /// Invalid Request
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// Unauthenticated
        /// </summary>
        Unauthenticated = 401,
        /// <summary>
        /// Unauthorized
        /// </summary>
        Unauthorized = 403,
        /// <summary>
        /// Resource not found
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// Internal Server Error
        /// </summary>
        Internal = 500,
        /// <summary>
        /// Resource not available
        /// </summary>
        Unavailable = 503

    }

    public class ErrorDetailsModel : SuperBaseModel
    {
        public HttpStatusCodeEnum HttpStatusCode { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }
        public string Message { get; set; }
        public ErrorDetailsModel InnerErrorDetails { get; set; }

        public enum ErrorTypeEnum
        {
            User = 1,
            Internal = 2,
            Network = 3
        }
    }
}
