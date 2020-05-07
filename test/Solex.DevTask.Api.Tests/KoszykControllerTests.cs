using System;
using Xunit;
using AutoFixture.Idioms;
using Solex.DevTask.Api.Controllers;
using Solex.DevTest.TestUtils;
using AutoFixture.Xunit2;
using Moq;
using Solex.DevTask.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Solex.DevTask.Api.Models;

namespace Solex.DevTask.Api.Tests
{
    public class KoszykControllerTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(KoszykController).GetConstructors());
        }

        [Theory, WebApiAutoMoqData]
        public void Get_ShouldReturnCorrectResult([Frozen] Mock<IKoszykService> koszykServiceMock, IEnumerable<ProduktModel> produkty, KoszykController sut)
        {
            // arrange
            koszykServiceMock.Setup(m => m.PobierzKoszyk()).Returns(produkty);

            // act
            var actual = sut.Get().Result as OkObjectResult;

            // assert
            actual.StatusCode.ShouldBe(200);
            actual.Value.ShouldBe(produkty);
        }
    }
}
