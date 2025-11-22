using AutoMapper;
using Ejada.SurveyManager.Indicators;
using Ejada.SurveyManager.Indicators.Dtos;
using Ejada.SurveyManager.SurveyInstances;
using Ejada.SurveyManager.SurveyInstances.Dtos;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Dtos;

//using Ejada.SurveyManager.Surveys.Dtos;
using System;

namespace Ejada.SurveyManager;

public class SurveyManagerApplicationAutoMapperProfile : Profile
{
    public SurveyManagerApplicationAutoMapperProfile()
    {
        // Survey mappings
        CreateMap<Survey, SurveyDto>()
            .ForMember(d => d.CreatedByUserId, opt => opt.MapFrom(s => s.CreatedByUserId));
        CreateMap<Survey, SurveyWithQuestionsDto>();
        
        // UpdateSurveyDto -> Survey: Required by base class, but actual mapping done in MapToEntity override
        CreateMap<UpdateSurveyDto, Survey>()
            .ForAllMembers(opt => opt.Ignore());
        
        // Question mappings
        CreateMap<Question, QuestionDto>();
        CreateMap<Option, OptionDto>();
        
        // CreateQuestionDto -> Question: Required by base class, but actual mapping done in MapToEntityAsync override
        CreateMap<CreateQuestionDto, Question>()
            .ForAllMembers(opt => opt.Ignore());
        
        // UpdateQuestionDto -> Question: Required by base class, but actual mapping done in UpdateAsync override
        CreateMap<UpdateQuestionDto, Question>()
            .ForAllMembers(opt => opt.Ignore());

        // SurveyInstance mappings
        CreateMap<SurveyInstance, SurveyInstanceDto>();
        CreateMap<Response, ResponseDto>();

        // Indicator mappings
        CreateMap<Indicator, IndicatorDto>()
            .ForMember(d => d.CreatedByUserId, opt => opt.MapFrom(s => s.CreatedByUserId))
            .ForMember(d => d.QuestionIds, opt => opt.Ignore()); // Loaded separately in AppService
    }
}
