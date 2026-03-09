using System.Net;

namespace Project.DigitalMarket.Libs.Exceptions
{
    public class AuthException : BaseHttpStatusCodeException
    {
        public AuthException(string message) : base(message)
        {
        }

        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Unauthorized;
    }
}
