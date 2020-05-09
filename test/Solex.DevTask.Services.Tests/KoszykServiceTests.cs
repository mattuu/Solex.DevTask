using System;
using System.Collections.Generic;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Shouldly;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Domain;
using Solex.DevTask.Interfaces;
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

        [Theory, AutoMoqData]
        public void DodajProdukt_ShouldAddProductToCart([Frozen] Mock<IKoszykRepository> koszykRepositoryMock, int id,
            decimal ilosc, KoszykService sut)
        {
            // act
            sut.DodajProdukt(id, ilosc);

            // assert
            koszykRepositoryMock.Verify(
                m => m.DodajProduktDoKoszyka(It.Is<int>(i => i == id), It.Is<decimal>(d => d == ilosc)), Times.Once());

        }

        [Theory, AutoMoqData]
        public void UsunProdukt_ShouldRemoveProductFromCart([Frozen] Mock<IKoszykRepository> koszykRepositoryMock, int id,
            KoszykService sut)
        {
            // act
            sut.UsunProdukt(id);

            // assert
            koszykRepositoryMock.Verify(m => m.UsunZKoszyka(It.Is<int>(i => i == id)), Times.Once());
        }

        [Theory, AutoMoqData]
        public void PobierzKoszyk_ShouldReturnCorrectResult([Frozen] Mock<IKoszykRepository> koszykRepositoryMock, [Frozen] Mock<IMapper> mapperMock, IEnumerable<Produkt> produkty, IEnumerable<ProduktModel> produktModele,
            KoszykService sut)
        {
            // arrange
            koszykRepositoryMock.Setup(m => m.PobierzKoszyk()).Returns(produkty);
            mapperMock.Setup(m => m.Map<IEnumerable<ProduktModel>>(produkty)).Returns(produktModele);

            // act
            var actual = sut.PobierzKoszyk();

            // assert
            koszykRepositoryMock.Verify(m => m.PobierzKoszyk(), Times.Once());
            actual.ShouldBe(produktModele);
        }
    }
}
