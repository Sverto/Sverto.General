using System;

namespace Sverto.General.Coding
{
    /// <summary>
    /// String version of Result for easier coding
    /// </summary>
    public class ResultS : Result<string>
    {
        public ResultS() : base() { }
        public ResultS(IResult result) : base(result) { }
        public ResultS(Result<string> result) : base(result) { }
        public ResultS(Exception value) : base(value) { }
        public ResultS(string value) : base(value) { }
    }
}
