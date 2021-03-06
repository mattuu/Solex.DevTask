﻿using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Solex.DevTest.TestUtils
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperMoqDataAttribute : AutoDataAttribute
    {
        public AutoMapperMoqDataAttribute()
            : base(CreateFixture)
        {
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMapperCustomization())
                .Customize(new AutoMoqCustomization())
                .Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}