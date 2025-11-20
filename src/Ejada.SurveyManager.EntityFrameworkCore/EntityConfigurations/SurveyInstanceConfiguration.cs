using Ejada.SurveyManager.SurveyInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ejada.SurveyManager.EntityConfigurations
{
    public class SurveyInstanceConfiguration : IEntityTypeConfiguration<SurveyInstance>
    {
        public void Configure(EntityTypeBuilder<SurveyInstance> builder)
        {
            builder.ToTable("SurveyInstances");

            builder.HasKey(si => si.Id);

            builder.Property(si => si.SurveyId).IsRequired();

            builder.Property(si => si.AssigneeUserId).IsRequired();

            builder.Property(si => si.DueDate).IsRequired(false);

            builder.Property(si => si.Status).IsRequired().HasConversion<string>();

            builder.HasIndex(si => si.SurveyId);
            builder.HasIndex(si => si.AssigneeUserId);

        }
    }
}
