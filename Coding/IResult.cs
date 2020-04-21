using System;

namespace Sverto.General.Coding
{
    public interface IResult
    {
        Exception Error { get; set; }
        int ErrorCode { get; set; }
        string ErrorMessage { get; set; }
    }
}
