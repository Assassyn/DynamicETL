namespace AraneaIt.Migration.Providers.Excel
{
    using AraneaIT.Migration.Engine;
    using AraneaIT.Migration.Engine.Actions;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;

    /// <summary>
    /// Binarty excel  Reader
    /// </summary>
    [DebuggerDisplay("Firebird reader:{entityName}")]
    [Reader("BinaryExcel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class BinaryExcelReader : Reader
    {
        protected override void AdditionalConfigure(IDictionary<string, string> parametersSet)
        {
            base.AdditionalConfigure(parametersSet);
        }

        public override Entity Read(Entity workingEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}
