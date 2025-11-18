using AutoMapper;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Dtos;
using System;

namespace Ejada.SurveyManager;

public class SurveyManagerApplicationAutoMapperProfile : Profile
{
    public SurveyManagerApplicationAutoMapperProfile()
    {
        CreateMap<Survey, SurveyDto>()
            .ForMember(d => d.CreatedByUserId, opt => opt.MapFrom(s => s.CreatedByUserId));
        CreateMap<Survey, SurveyWithQuestionsDto>();

        // Question (Domain -> DTO)
        CreateMap<Question, QuestionDto>();

        // Question (Create DTO -> Domain)
        CreateMap<CreateQuestionDto, Question>()
            .ConstructUsing(src =>
                Question.Create(
                    Guid.NewGuid(),
                    src.Text,
                    src.Type
                ));

        // Question (Update DTO -> Domain)
        CreateMap<UpdateQuestionDto, Question>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Option, OptionDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
