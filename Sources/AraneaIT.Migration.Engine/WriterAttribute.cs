// <copyright file="WriterAttribute.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///MEF attribute used to lcoate writer
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), MetadataAttribute]
    public sealed class WriterAttribute : ExportAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriterAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public WriterAttribute(string type)
            : base(type.ToLower(), typeof(IWriter))
        {
        }
    }
}