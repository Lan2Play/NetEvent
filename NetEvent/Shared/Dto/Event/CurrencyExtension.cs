using System;

namespace NetEvent.Shared.Dto.Event
{
    public static class CurrencyExtension
    {
        public static string ToSymbol(this CurrencyDto currency)
        {
            return currency switch
            {
                CurrencyDto.Euro => "€",
                _ => throw new NotSupportedException($"The Currency {currency} is not implemented!"),
            };
        }

        public static string To3DigitIso(this CurrencyDto currency)
        {
            return currency switch
            {
                CurrencyDto.Euro => "EUR",
                _ => throw new NotSupportedException($"The Currency {currency} is not implemented!"),
            };
        }

        public static int ToCurrencyBaseValue(this CurrencyDto currency, double value)
        {
            return currency switch
            {
                CurrencyDto.Euro => (int)(value * 100),
                _ => throw new NotSupportedException($"The Currency {currency} is not implemented!"),
            };
        }

        public static double ToCurrencyValue(this CurrencyDto currency, int baseValue)
        {
            return currency switch
            {
                CurrencyDto.Euro => baseValue / 100d,
                _ => throw new NotSupportedException($"The Currency {currency} is not implemented!"),
            };
        }
    }
}
