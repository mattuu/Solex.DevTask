using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.Idioms;
using AutoMapper;
using SemanticComparison.Fluent;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Domain;
using Solex.DevTask.Services.Profiles;
using Solex.DevTest.TestUtils;
using Xunit;

namespace Solex.DevTask.Services.Tests.Profiles
{
    public class ProduktToProduktModelProfileTests
    {
        [Theory]
        [AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(ProduktToProduktModelProfile).GetConstructors());
        }

        [Theory, AutoMapperMoqData]
        public void Map_ShouldReturnCorrectResult(Produkt produkt, IMapper mapper)
        {
            // act
            var actual = mapper.Map<ProduktModel>(produkt);

            // assert
            produkt.AsSource()
                .OfLikeness<ProduktModel>()
                .With(m => m.Id).EqualsWhen((p, m) => p.Id == m.Id)
                .With(m => m.Ilosc).EqualsWhen((p, m) => p.Ilosc == m.Ilosc)
                .ShouldEqual(actual);
        }
    }
}
