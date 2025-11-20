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
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable("Options");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.QuestionId)
                .IsRequired();

            builder.Property(o => o.Label)
                .IsRequired()
                .HasMaxLength(DomainConstants.OptionLabelMaxLength);

            builder.Property(o => o.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.HasIndex(o => o.QuestionId);

            builder.HasOne<Question>()
                .WithMany(q => q.Options)
                .HasForeignKey(o=>o.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
