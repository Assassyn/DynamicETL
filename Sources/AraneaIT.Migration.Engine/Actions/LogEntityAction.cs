// <copyright file="LogEntityAction.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.ComponentModel.Composition;

    [Converter("logger")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class LogEntityAction : Converter
    {
        public override Entity Perform(Entity currentEntity)
        {
            this.Logger.Info("Logger: Entity properties listing");

            foreach (var property in currentEntity.PropertiesDictionary)
            {
                this.Logger.Info("Logger: property =>{0}<= has value =>{1}<=",
                    property.Key,
                    property.Value);
            }
            this.Logger.Info("Logger: Entity properties finished");

            return currentEntity;
        }
    }
}