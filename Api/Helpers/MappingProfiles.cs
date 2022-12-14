using Infrastructure.DTOs;
using AutoMapper;
using Core.Data.EntryDbModels;
using Type = Core.Data.EntryDbModels.Type;

namespace Api.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<EntryCategoryViewModel, Category>();
        CreateMap<EntryTypeModel, Type>();
        CreateMap<Type, TypeViewModel>();
        CreateMap<TypeViewModel, Type>();
        CreateMap<Category, CategoryViewModel>();
        CreateMap<CategoryViewModel, Category>();
        // CreateMap<EntryProductDto, Product>();
        // CreateMap<Product, ProductViewModel>().ForMember(x => x.Category, 
        //     s => s.MapFrom(z => z.Category!.Name));
    }
}