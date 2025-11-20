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
    public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder.ToTable("Surveys");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(DomainConstants.SurveyNameMaxLength);

            builder.Property(s => s.Purpose)
                .IsRequired(false)
                .HasMaxLength(DomainConstants.SurveyPurposeMaxLength);

            builder.Property(s => s.TargetAudience)
                .IsRequired(false)
                .HasMaxLength(DomainConstants.SurveyTargetAudienceMaxLength);

            builder.Property(s => s.IsActive)
                .IsRequired();
        }
    }
}
