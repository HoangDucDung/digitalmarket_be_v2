using Project.DigitalMarket.Libs.Constants.ErrorCode;
using System.Net;

namespace Project.DigitalMarket.Libs.Exceptions
{
    public class BusinessException : BaseHttpStatusCodeException
    {
        public BusinessException(string message, int errorCode = 0, HttpStatusCode httpStatus = HttpStatusCode.UnprocessableContent) : base(errorCode, message)
        {
            StatusCode = httpStatus;
        }

        public BusinessException(int errorCode, string message, HttpStatusCode httpStatus = HttpStatusCode.UnprocessableContent) : base(errorCode, message)
        {
            StatusCode = httpStatus;
        }

        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.UnprocessableContent;
    }
}
