// <copyright file="TypeExtension.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extending type to create custom object for writers and riders
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Creates the object based on entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="entityDefinition">The entity definition.</param>
        /// <param name="currentEntity">The current entity.</param>
        /// <returns>Custom, type defined object, with data populated from Entity.</returns>
        /// <exception cref="System.NullReferenceException">Entity do not provide required value</exception>
        public static object CreateObjectBasedOnEntity(this Type type, IEntityDefinition entityDefinition, Entity currentEntity)
        {
            var result = Activator.CreateInstance(type);

            foreach (var propertyDefinition in entityDefinition.PropertiesDefinition)
            {
                if (!currentEntity.PropertiesDictionary.ContainsKey(propertyDefinition.SourceName))
                {
                    throw new NullReferenceException("Entity do not provide required value for name: " + propertyDefinition.Name);
                }

                if (propertyDefinition.Name.Contains("."))
                {
                    var propertiesTree = propertyDefinition.Name.Split('.');

                    PropertyInfo propertyInfo = null;
                    var currentType = type;
                    var workingObject = result;
                    var last = propertiesTree.Length;

                    for (int index = 0; index < last; ++index)
                    {
                        var propertyContainer = propertiesTree[index];
                        propertyInfo = currentType.GetProperty(propertyContainer);

                        if (propertyInfo.GetValue(workingObject, null) == null)
                        {
                            var missingPropertyValue = Activator.CreateInstance(propertyInfo.PropertyType);
                            propertyInfo.SetValue(workingObject, missingPropertyValue, null);
                        }

                        currentType = propertyInfo.PropertyType;

                        if (index + 1 < last)
                        {
                            workingObject = propertyInfo.GetValue(workingObject, null);
                        }
                    }

                    if (propertyInfo != null)
                    {
                        var value = currentEntity[propertyDefinition.SourceName];
                        propertyInfo.SetValue(workingObject, value, null);
                    }
                }
                else
                {
                    var property = type.GetProperty(propertyDefinition.Name);
                    var value = currentEntity[propertyDefinition.SourceName];
                    property.SetValue(result, value, null);
                }
            }

            return result;
        }
    }
}