// <copyright file="ReadCondition.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;

    /// <summary>
    /// Allows to define the additional parameters for read process
    /// </summary>
    public sealed class ReadCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCondition" /> class.
        /// </summary>
        public ReadCondition()
        {
            this.AddQuotes = false;
            this.ValueFormat = "dd/mm/yyyy";
            this.QuoteCharacter = "'";
        }

        /// <summary>
        /// Gets or sets the search key.
        /// </summary>
        /// <value>
        /// The search key.
        /// </value>
        public string SearchKey { get; set; }

        /// <summary>
        /// Gets or sets the comparer.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        public string Comparer { get; set; }

        /// <summary>
        /// Gets or sets the value. Possible formats:
        /// <list>
        /// <item>fixed value</item>
        /// <item>{today}</item>
        /// <item>@{reference name}</item>
        /// </list>
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [add quotes].
        /// Default value is false
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add quotes]; otherwise, <c>false</c>.
        /// </value>
        public bool AddQuotes { get; set; }

        /// <summary>
        /// Gets or sets the quote character.
        /// </summary>
        /// <value>
        /// The quote character.
        /// </value>
        public string QuoteCharacter { get; set; }

        /// <summary>
        /// Gets or sets the additional formatting.
        /// </summary>
        /// <value>
        /// The additional formatting.
        /// </value>
        public string ValueFormat { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>value of the entity</returns>
        public string GetValue(Entity entity = null)
        {
            var result = this.Value;

            if (string.IsNullOrEmpty(this.Value))
            {
                result = string.Empty;
            }
            else if (string.Equals(this.Value, "{today}", StringComparison.OrdinalIgnoreCase))
            {
                result = DateTime.Now.ToString(this.ValueFormat);
            }
            else if (this.Value.StartsWith("@{") && this.Value.EndsWith("}"))
            {
                var propertyName = this.Value.Substring(2, this.Value.Length - 3);
                var referenceValue = entity[propertyName];

                if (referenceValue is DateTime)
                {
                    result = ((DateTime)referenceValue).ToString(this.ValueFormat);
                }
                else
                {
                    result = referenceValue.ToString();
                }
            }

            if (this.AddQuotes)
            {
                result = string.Concat(
                    this.QuoteCharacter,
                    result,
                    this.QuoteCharacter);
            }

            return result;
        }
    }
}