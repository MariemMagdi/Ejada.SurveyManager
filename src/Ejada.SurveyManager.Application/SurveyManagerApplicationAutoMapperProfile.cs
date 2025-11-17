using AutoMapper;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Dtos;

namespace Ejada.SurveyManager;

public class SurveyManagerApplicationAutoMapperProfile : Profile
{
    public SurveyManagerApplicationAutoMapperProfile()
    {
        CreateMap<Survey, SurveyDto>()
            .ForMember(d => d.CreatedByUserId, opt => opt.MapFrom(s => s.CreatedByUserId));
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
