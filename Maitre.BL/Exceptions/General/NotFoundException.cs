using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Maitre.BL.Exceptions.General
{
    public class NotFoundException : Exception, IBaseException
    {
        public int StatusCode => HttpStatusCode.NotFound.GetHashCode();

        public string ErrorMessage { get; }
        public NotFoundException(string message) : base(message)
        {
            ErrorMessage = message;
        }
    }
}
