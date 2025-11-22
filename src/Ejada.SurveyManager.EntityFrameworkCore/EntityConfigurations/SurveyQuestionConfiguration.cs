using Ejada.SurveyManager.EntityFrameworkCore.Helpers;
using Ejada.SurveyManager.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.SurveyManager.EntityConfigurations
{
    public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
    {
        public void Configure(EntityTypeBuilder<SurveyQuestion> builder) 
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(SurveyQuestion)));

            builder.HasKey(sq => sq.Id);

            builder.Property(sq => sq.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyQuestion.Id)));

            builder.Property(sq => sq.SurveyId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyQuestion.SurveyId)));

            builder.Property(sq => sq.QuestionId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyQuestion.QuestionId)));

            builder.HasIndex(sq => new { sq.SurveyId, sq.QuestionId })
                .IsUnique();

            builder.HasOne<Survey>()
                .WithMany()
                .HasForeignKey(sq=>sq.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Question>()
                .WithMany()
                .HasForeignKey(sq => sq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
