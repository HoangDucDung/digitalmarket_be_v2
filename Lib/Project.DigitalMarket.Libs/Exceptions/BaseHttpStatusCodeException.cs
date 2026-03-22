using System.Net;

namespace Project.DigitalMarket.Libs.Exceptions
{
    public abstract class BaseHttpStatusCodeException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; set; }
        public int ErrorCode { get; set; }

        public BaseHttpStatusCodeException(string message) : base(message)
        {
        }

        public BaseHttpStatusCodeException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
