using System;
using System.Net;
using System.Text;

namespace Sverto.General.Coding
{
    public class Result<T> : IResult
    {
        public Result()
        {
        }
        public Result(T value)
        {
            Value = value;
        }
        public Result(Exception ex)
        {
            Error = ex;
            ErrorMessage = ex.Message;

            if (ex is WebException)
            {
                var webEx = ex as WebException;
                if (webEx.Status == WebExceptionStatus.ProtocolError && webEx.Response != null)
                    ErrorCode = (int)((HttpWebResponse)webEx.Response).StatusCode;
            }
        }
        public Result(Result<T> result) : this(result as IResult)
        {
            Value = result.Value;
        }
        public Result(IResult result)
        {
            Error = result.Error;
            ErrorCode = result.ErrorCode;
            if (result.Error.Message != result.ErrorMessage)
                ErrorMessage = result.ErrorMessage;
        }

        /// <summary>
        /// The result value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// The result Exception
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// The result error code
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Returns the Exception errormessage unless set otherwise
        /// </summary>
        private string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage ?? Error?.Message; }
            set { _ErrorMessage = value; }
        }

        /// <summary>
        /// Returns true if Error or ErrorMessage is set, or when Errorcode is not 0
        /// </summary>
        public bool IsError
        {
            get { return Error != null || ErrorMessage != null || ErrorCode != 0; }
        }

        public override string ToString()
        {
            var b = new StringBuilder();
            b.Append("Value: ");
            b.AppendLine(Value?.ToString());
            if (IsError)
            {
                b.Append("Error Code: ");
                b.AppendLine(ErrorCode.ToString());
                b.Append("Error Message: ");
                b.AppendLine(ErrorMessage);
                b.Append("Error: ");
                b.AppendLine(Error?.ToString());
            }
            return b.ToString();
        }
    }


    public class Result<T, T2> : Result<T>
    {
        public Result(IResult result) : base(result) { }
        public Result(Result<T> result) : base(result) { }
        public Result(Result<T2> result) : base(result as IResult)
        {
            Value2 = result.Value;
        }
        public Result(Exception value) : base(value) { }
        public Result(T value) : base(value) { }
        public Result(T value, T2 value2) : base(value)
        {
            Value2 = value2;
        }
        public Result(T2 value)
        {
            Value2 = value;
        }

        /// <summary>
        /// A second result value
        /// </summary>
        public T2 Value2 { get; set; }
    }
}
