// <copyright file="EntityExtension.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.Linq;

    /// <summary>
    /// EEntity extension
    /// </summary>
    public static class EntityExtension
    {
        /// <summary>
        /// Updates the values.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="valuesSet">The values set.</param>
        /// <returns>Entity with values updated</returns>
        public static Entity UpdateValues(this Entity entity, object valuesSet)
        {
            if (valuesSet != null)
            {
                var typeInfo = valuesSet.GetType();

                foreach (var propertyInfo in typeInfo.GetProperties().Where(x => x.CanRead))
                {
                    entity.AddProperty(
                        propertyInfo.Name,
                        propertyInfo.PropertyType.FullName,
                        propertyInfo.GetValue(valuesSet, null));
                }
            }

            return entity;
        }
    }
}