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

        [Theory]
        [InlineAutoMoqData(1, 10.1, 2, 9.9, 150.5)]
        [InlineAutoMoqData(1, 10.1, 0, 0, 101)]
        [InlineAutoMoqData(1, 9.9, 0, 0, 49.5)]
        public void PobierzKoszykWartosc_ShouldReturnCorrectValue(int id1, decimal ilosc1, int id2, decimal ilosc2, decimal wartosc, [Frozen] Mock<IKoszykRepository> koszykRepositoryMock, KoszykService sut)
        {
            // arrange
            var produkty = new HashSet<Produkt>();
            if (id1 != default)
            {
                produkty.Add(new Produkt() {Id = id1, Ilosc = ilosc1});
            };
            if (id2 != default)
            {
                produkty.Add(new Produkt() { Id = id2, Ilosc = ilosc2 });
            };

            koszykRepositoryMock.Setup(m => m.PobierzKoszyk()).Returns(produkty);

            // act
            var actual = sut.PobierzKoszykWartosc();

            // assert
            actual.ShouldBe(wartosc);
        }

        [Theory, AutoMoqData]
        public void PobierzKoszykWartosc_ShouldZeroWhenCartIsEmpty([Frozen] Mock<IKoszykRepository> koszykRepositoryMock, KoszykService sut)
        {
            // arrange
            koszykRepositoryMock.Setup(m => m.PobierzKoszyk()).Returns(new Produkt[0]);

            // act
            var actual = sut.PobierzKoszykWartosc();

            // assert
            actual.ShouldBe(0m);
        }

    }
}
