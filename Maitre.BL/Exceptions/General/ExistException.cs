using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Maitre.BL.Exceptions.General;

public class ExistException : Exception, IBaseException
{
    public int StatusCode => HttpStatusCode.Conflict.GetHashCode();

    public string ErrorMessage { get; }
    public ExistException(string message) : base(message)
    {
        ErrorMessage = message;
    }
}
