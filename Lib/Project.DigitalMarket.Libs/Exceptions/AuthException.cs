using System.Net;

namespace Project.DigitalMarket.Libs.Exceptions
{
    public class AuthException : BaseHttpStatusCodeException
    {
        public AuthException(string message, int errorCode = 0) : base(errorCode, message)
        {
        }

        public AuthException(int errorCode, string message) : base(errorCode, message)
        {
        }

        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Unauthorized;
    }
}
