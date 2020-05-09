using System.Collections.Generic;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Solex.DevTask.Api.Controllers;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Domain.Exceptions;
using Solex.DevTask.Interfaces;
using Solex.DevTest.TestUtils;
using Xunit;

namespace Solex.DevTask.Api.Tests
{
    public class KoszykControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(KoszykController).GetConstructors());
        }

        [Theory]
        [WebApiAutoMoqData]
        public void Get_ShouldReturnOkResult_WhenServiceReturnsData([Frozen] Mock<IKoszykService> koszykServiceMock,
            IEnumerable<ProduktModel> produkty, KoszykController sut)
        {
            // arrange
            koszykServiceMock.Setup(m => m.PobierzKoszyk()).Returns(produkty);

            // act
            var actual = sut.Get().Result;

            // assert
            actual.ShouldBeOfType<OkObjectResult>();
            (actual as OkObjectResult).Value.ShouldBe(produkty);
        }

        [Theory]
        [WebApiAutoMoqData]
        public void Get_ShouldReturnNotFoundResult_WhenServiceReturnsNoData(
            [Frozen] Mock<IKoszykService> koszykServiceMock, KoszykController sut)
        {
            // arrange
            koszykServiceMock.Setup(m => m.PobierzKoszyk()).Returns(default(IEnumerable<ProduktModel>));

            // act
            var actual = sut.Get().Result;

            // assert
            actual.ShouldBeOfType<NotFoundResult>();
        }

        [Theory]
        [WebApiAutoMoqData]
        public void DodajProduktDoKoszyka_ShouldReturnCreated_WhenModelInvalid(DodajProduktModel model,
            KoszykController sut)
        {
            // act
            var actual = sut.DodajProduktDoKoszyka(model);

            // assert
            actual.ShouldBeOfType<StatusCodeResult>();
            (actual as StatusCodeResult).StatusCode.ShouldBe(201);
        }

        [Theory]
        [WebApiAutoMoqData]
        public void DodajProduktDoKoszyka_ShouldAddProductToCart_WhenModelIsValid(
            [Frozen] Mock<IKoszykService> koszykServiceMock, DodajProduktModel model, KoszykController sut)
        {
            // act
            var actual = sut.DodajProduktDoKoszyka(model);

            // assert
            koszykServiceMock.Verify(
                m => m.DodajProdukt(It.Is<int>(i => i == model.Id), It.Is<decimal>(d => d == model.Ilosc)),
                Times.Once());
        }

        [Theory]
        [WebApiAutoMoqData]
        public void DodajProduktDoKoszyka_ShouldReturnBadRequest_WhenModelIsInvalid(string key, string error,
            DodajProduktModel model, KoszykController sut)
        {
            // arrange
            sut.ModelState.AddModelError(key, error);

            // act
            var actual = sut.DodajProduktDoKoszyka(model);

            // assert
            actual.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [WebApiAutoMoqData]
        public void DodajProduktDoKoszyka_ShouldNotAddProductToCart_WhenModelIsNotValid(
            [Frozen] Mock<IKoszykService> koszykServiceMock, DodajProduktModel model, string key, string error,
            KoszykController sut)
        {
            // arrange
            sut.ModelState.AddModelError(key, error);

            // act
            var actual = sut.DodajProduktDoKoszyka(model);

            // assert
            koszykServiceMock.Verify(m => m.DodajProdukt(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never());
        }

        [Theory]
        [WebApiAutoMoqData]
        public void UsunZKoszyka_ShouldReturnNoContent_WhenItemRemoved(
            [Frozen] Mock<IKoszykService> koszykServiceMock, int id, KoszykController sut)
        {
            // act
            var actual = sut.UsunZKoszyka(id);

            // assert
            actual.ShouldBeOfType<NoContentResult>();
        }

        [Theory]
        [WebApiAutoMoqData]
        public void UsunZKoszyka_ShouldReturnNotFound_WhenItemNotExist(
            [Frozen] Mock<IKoszykService> koszykServiceMock, int id, KoszykController sut)
        {
            // arrange
            koszykServiceMock.Setup(m => m.UsunProdukt(It.Is<int>(i => i == id))).Throws<ItemNotFoundException>();

            // act
            var actual = sut.UsunZKoszyka(id);

            // assert
            actual.ShouldBeOfType<NotFoundObjectResult>();
        }

        [Theory]
        [WebApiAutoMoqData]
        public void UsunZKoszyka_ShouldRemoveItem_WhenItemExists(
            [Frozen] Mock<IKoszykService> koszykServiceMock, int id, KoszykController sut)
        {
            // act
            var actual = sut.UsunZKoszyka(id);

            // assert
            koszykServiceMock.Verify(m => m.UsunProdukt(It.Is<int>(i => i == id)), Times.Once());
        }

        [Theory, WebApiAutoMoqData]
        public void PobierzKoszykWartosc_ShouldReturnCorrectResult([Frozen] Mock<IKoszykService> koszykServiceMock, decimal koszykWartosc,
            KoszykController sut)
        {
            // arrange
            koszykServiceMock.Setup(m => m.PobierzKoszykWartosc()).Returns(koszykWartosc);

            // act
            var actual = sut.PobierzKoszykWartosc();

            // assert
            actual.ShouldBeOfType<OkObjectResult>();
            (actual as OkObjectResult).Value.ShouldBe(koszykWartosc);
        }
    }
}