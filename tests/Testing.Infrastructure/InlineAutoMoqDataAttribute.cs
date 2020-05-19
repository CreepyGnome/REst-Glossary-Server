﻿using System;
using AutoFixture.Xunit2;
using Xunit;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Provides behavior to auto-generate dependencies like <seealso cref="AutoMoqDataAttribute"/> as well as behavior to run a theory with different sets of data like <seealso cref="InlineDataAttribute"/>.
    /// </summary>
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        /// <summary>
        /// Creates a InlineAutoMoqDataAttribute that creates a fixture with the default customizations.
        /// <para>The data values to pass to the theory must appear first in the method's parameter list. Auto-generated dependencies should be last in the list.</para>
        /// </summary>
        /// <param name="objects">The data values to pass to the theory.</param>
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects) { }

        /// <summary>
        /// Creates a InlineAutoMoqDataAttribute that creates a fixture with the specified behavior.
        /// <para>The data values to pass to the theory must appear first in the method's parameter list. Auto-generated dependencies should be last in the list.</para>
        /// </summary>
        /// <param name="configureMembers">
        /// Specifies whether members of a mock will be automatically setup to retrieve the return values from a fixture.
        /// </param>
        /// <param name="generateDelegates">
        /// If value is <c>true</c>, delegate requests are intercepted and created by Moq.
        /// Otherwise, if value is <c>false</c>, delegates are created by the AutoFixture kernel.
        /// </param>
        /// <param name="recursionDepth">
        /// If value is greater than <c>0</c>, circular dependencies will only be generated by AutoFixture up to the specified depth (1 will not generate any circular dependencies).
        /// Otherwise, if value is <c>0</c>, AutoFixture will attempt to generate circular dependencies and will throw exceptions if unable to do so.
        /// </param>
        /// <param name="attributeTypesToIgnoreForProperties">
        /// If any of the types provided are an attribute decorating a property they will be ignored and thereby not set.
        /// </param>
        /// <param name="objects">The data values to pass to the theory.</param>
        public InlineAutoMoqDataAttribute(bool configureMembers = true, bool generateDelegates = false, int recursionDepth = 0, Type[] attributeTypesToIgnoreForProperties = null, Type[] propertyTypesToIgnore = null, params object[] objects)
            : base(new AutoMoqDataAttribute(configureMembers, generateDelegates, recursionDepth, attributeTypesToIgnoreForProperties, propertyTypesToIgnore), objects) { }
    }
}
