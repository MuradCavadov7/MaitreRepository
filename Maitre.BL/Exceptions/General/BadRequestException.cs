using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Maitre.BL.Exceptions.General;

public class BadRequestException : Exception, IBaseException
{
    public int StatusCode => HttpStatusCode.BadRequest.GetHashCode();

    public string ErrorMessage { get; }
    public BadRequestException(string message) : base(message)
    {
        ErrorMessage = message;
    }
}
