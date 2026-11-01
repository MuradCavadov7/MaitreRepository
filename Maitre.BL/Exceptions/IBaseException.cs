using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Exceptions;

public interface IBaseException 
{
    int StatusCode { get; }
    string ErrorMessage { get; }
}
