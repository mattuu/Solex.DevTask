using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoMapper;
using Solex.DevTask.Services.Profiles;

namespace Solex.DevTest.TestUtils
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<IMapper>(() => new MapperConfiguration(cfg => cfg.AddProfile<ProduktToProduktModelProfile>()).CreateMapper());
        }
    }
}