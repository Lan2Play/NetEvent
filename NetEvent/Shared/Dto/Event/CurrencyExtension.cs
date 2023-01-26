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
    }
}
