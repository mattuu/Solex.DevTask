using AutoMapper;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Domain;

namespace Solex.DevTask.Services.Profiles
{
    public class ProduktToProduktModelProfile : Profile
    {
        public ProduktToProduktModelProfile()
        {
            CreateMap<Produkt, ProduktModel>();
        }
    }
}