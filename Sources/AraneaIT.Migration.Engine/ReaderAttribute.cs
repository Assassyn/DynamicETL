// <copyright file="ReaderAttribute.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// MEF attribute used to locate reader
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), MetadataAttribute]
    public sealed class ReaderAttribute : ExportAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ReaderAttribute(string type)
            : base(type.ToLower(), typeof(IReader))
        {
        }
    }
}