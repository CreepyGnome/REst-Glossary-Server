using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Omits any property of an excluded type.
    /// </summary>
    public class OmitOnPropertyTypeSpecimenBuilder : ISpecimenBuilder
    {
        /// <summary>
        /// The types that should be excluded.
        /// </summary>
        public IEnumerable<Type> ExcludedTypes { get; set; }

        /// <inheritdoc/>
        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is Type propInfo))
                return new NoSpecimen();
            
            if (ExcludedTypes?.Any() == true && ExcludedTypes.Contains(propInfo))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}