using Api.Helpers;
using AutoMapper;

namespace ShopProject.Test.Mocks;

public static class MapperMock
{
    public static IMapper Instance
    {
        get
        {
            var mapperConfig = new MapperConfiguration(
                config => config.AddProfile<MappingProfiles>());

            return new Mapper(mapperConfig);
        }
    }
}