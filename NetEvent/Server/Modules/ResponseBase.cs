﻿namespace NetEvent.Server.Modules
{
    public class ResponseBase : ResponseBase<object>
    {
        public ResponseBase() : base(null)
        {
        }

        public ResponseBase(ReturnType returnType, string error) : base(returnType, error)
        {
        }
    }

    public class ResponseBase<T>
    {
        public ResponseBase(T? value)
        {
            ReturnValue = value;
            ReturnType = ReturnType.Ok;
        }

        public ResponseBase(ReturnType returnType, string error)
        {
            ReturnType = returnType;
            Error = error;
        }


        public ReturnType ReturnType { get; set; }

        public string? Error { get; set; }

        public T? ReturnValue { get; set; }

    }
}