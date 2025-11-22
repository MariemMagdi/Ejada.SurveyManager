using Ejada.SurveyManager.EntityFrameworkCore.Helpers;
using Ejada.SurveyManager.Indicators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ejada.SurveyManager.EntityConfigurations
{
    public class QuestionIndicatorConfiguration : IEntityTypeConfiguration<QuestionIndicator>
    {
        public void Configure(EntityTypeBuilder<QuestionIndicator> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(QuestionIndicator)));

            builder.HasKey(qi => new { qi.QuestionId, qi.IndicatorId });

            builder.Property(qi => qi.QuestionId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(QuestionIndicator.QuestionId)));

            builder.Property(qi => qi.IndicatorId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(QuestionIndicator.IndicatorId)));

            builder.HasIndex(qi => qi.QuestionId);
            builder.HasIndex(qi => qi.IndicatorId);
        }
    }
}

