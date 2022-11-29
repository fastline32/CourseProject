using Api.Models.DTOs;
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
        CreateMap<Category, CategoryViewModel>();
    }
}