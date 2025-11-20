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
            builder.ToTable("SurveyQuestions");

            builder.HasKey(sq => sq.Id);

            builder.Property(sq => sq.SurveyId).IsRequired();

            builder.Property(sq => sq.QuestionId).IsRequired();

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
