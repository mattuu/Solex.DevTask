using System;
using AutoFixture.Idioms;
using Solex.DevTest.TestUtils;
using Xunit;

namespace Solex.DevTask.Services.Tests
{
    public class KoszykServiceTests
    {
        [Theory]
        [AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(KoszykService).GetConstructors());
        }
    }
}
