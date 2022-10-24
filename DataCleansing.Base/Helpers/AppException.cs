using System;
using System.Globalization;

namespace DataCleansing.Base.Helpers
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message)
        {
            Args = null;
        }

        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            Args = args;
        }

        public object[] Args { get; }
    }
}
