using System;
using System.Globalization;
using NetEvent.Shared.Dto.Event;

namespace NetEvent.Client.Extensions
{
    public class CurrencyConverter : MudBlazor.Converter<int, string>
    {
        private readonly CurrencyDto _Currency;

        public CurrencyConverter(CurrencyDto currency)
        {
            SetFunc = ToInputValue;
            GetFunc = ToCurrencyValue;
            _Currency = currency;
        }

        private int ToCurrencyValue(string arg)
        {
            if (!double.TryParse(arg, CultureInfo.InvariantCulture, out var value))
            {
                return 0;
            }

            return _Currency.ToCurrencyBaseValue(value);
        }

        private string ToInputValue(int arg)
        {
            return Convert.ToString(_Currency.ToCurrencyValue(arg), Culture);
        }
    }
}
