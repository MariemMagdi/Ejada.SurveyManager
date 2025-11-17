using Ejada.SurveyManager.Common;
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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(DomainConstants.QuestionTextMaxLength);

            builder.Property(q => q.Type)
                .IsRequired()
                .HasConversion<int>();

            // Use field-backed collection
            builder.Navigation(q => q.Options).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(nameof(Question.Options)).UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
