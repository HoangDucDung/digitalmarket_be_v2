using System.Net;

namespace Project.DigitalMarket.Libs.Exceptions
{
    public abstract class BaseHttpStatusCodeException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; set; }

        public BaseHttpStatusCodeException(string message) : base(message)
        {
        }
    }
}
