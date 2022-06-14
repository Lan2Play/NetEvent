using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace NetEvent.Client.Components
{

    public class EventCallbackArgs<T>
    {
        public EventCallbackArgs(T value)
        {
            Value = value;
        }

        public bool Cancel { get; set; }

        public T Value { get; }
    }
}
