using AutoMapper;
using Ejada.SurveyManager.Surveys.Dtos;

namespace Ejada.SurveyManager.Blazor.Client;

public class SurveyManagerBlazorAutoMapperProfile : Profile
{
    public SurveyManagerBlazorAutoMapperProfile()
    {
        // Provide client-side mappings required by AbpCrudPageBase (Blazor)
        // so mapping SurveyDto -> UpdateSurveyDto (and CreateSurveyDto) exists on the client.
        CreateMap<SurveyDto, UpdateSurveyDto>();
        CreateMap<SurveyDto, CreateSurveyDto>();
    }
}