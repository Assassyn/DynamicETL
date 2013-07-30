// <copyright file="ConversionHelper.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    /// <summary>
    /// Helps to covert string to proepr value
    /// </summary>
    public static class ConversionHelper
    {
        /// <summary>
        /// Gets as boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>boolean converted value</returns>
        public static bool ToBoolean(this string value)
        {
            var result = false;

            bool.TryParse(value, out result);

            return result;
        }

        /// <summary>
        /// Gets as boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>int converted value</returns>
        public static int ToInt32(this string value)
        {
            var result = 0;

            int.TryParse(value, out result);

            return result;
        }
    }
}
