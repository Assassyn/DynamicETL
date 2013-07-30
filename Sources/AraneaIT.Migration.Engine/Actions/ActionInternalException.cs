// <copyright file="ActionInternalException.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class ActionInternalException : Exception
    {
        public ActionInternalException(string message)
            : base(message)
        {

        }
    }
}
