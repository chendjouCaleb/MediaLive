using System;

namespace MediaLive.Helpers
{
    public static class StringHelper
    {
        public static string Normalize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Normalize().ToUpperInvariant();
        }
    }
}