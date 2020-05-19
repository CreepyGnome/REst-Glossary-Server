using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Omits any property decorated with the provided attribute types.
    /// </summary>
    public class PropertyWithAttributeOmitterBuilder : ISpecimenBuilder
    {
        /// <summary>
        /// The types that if decorating a property should be excluded.
        /// </summary>
        public IList<Type> ExcludedTypes { get; set; }

        /// <inheritdoc/>
        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is PropertyInfo propInfo))
                return new NoSpecimen();
            
            if (ExcludedTypes == null || ExcludedTypes.Count == 0)
                return new NoSpecimen();

            var attributeTypes = propInfo.GetCustomAttributes().Select(a => a.GetType());

            if (attributeTypes.Intersect(ExcludedTypes).Any())
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}