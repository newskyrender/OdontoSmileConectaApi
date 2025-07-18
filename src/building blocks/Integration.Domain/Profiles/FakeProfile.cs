using AutoMapper;
using Integration.Domain.Entities;
using Integration.Domain.Http.Response;

namespace Integration.Domain.Profiles
{
    public class FakeProfile : Profile
    {
        public FakeProfile()
        {
            CreateMap<Fake, FakeResponse>()
                .ReverseMap();
        }
    }
}

