using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace MicroservicesSample.Common.Extensions
{
    /// <summary>
    /// Расширения для работы со строками
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Преобразование входной строки к нотации snack_case.
        /// </summary>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <returns>Преобразованная строка</returns>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+", RegexOptions.Compiled);
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2", RegexOptions.Compiled)
                .ToLower(CultureInfo.InvariantCulture);
        }
    }
}
