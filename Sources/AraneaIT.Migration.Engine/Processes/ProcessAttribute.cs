// <copyright file="ProcessAttribute.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Proceses
{
    using System;
    using System.ComponentModel.Composition;

    [AttributeUsage(AttributeTargets.Class), MetadataAttribute]
    public sealed class ProcessAttribute : ExportAttribute
    {
        public ProcessAttribute(string name)
            : base(name, typeof(Process))
        {
        }
    }
}