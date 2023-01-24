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
    }
}
