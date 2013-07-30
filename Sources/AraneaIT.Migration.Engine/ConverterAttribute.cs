// <copyright file="ConverterAttribute.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// MEF attribute to define converter object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), MetadataAttribute]
    public sealed class ConverterAttribute : ExportAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ConverterAttribute(string type)
            : base(type.ToLower(), typeof(IConverter))
        {
        }
    }
}