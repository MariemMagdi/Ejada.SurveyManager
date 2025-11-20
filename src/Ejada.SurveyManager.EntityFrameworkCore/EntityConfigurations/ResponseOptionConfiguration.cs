using Ejada.SurveyManager.SurveyInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.SurveyManager.EntityConfigurations
{
    public class ResponseOptionConfiguration : IEntityTypeConfiguration<ResponseOption>
    {
        public void Configure(EntityTypeBuilder<ResponseOption> builder)
        {
            builder.ToTable("ResponseOptions");
            builder.HasKey(ro => ro.Id);

            builder.Property(ro => ro.ResponseId).IsRequired();
            builder.Property(ro => ro.OptionId).IsRequired();

            builder.HasIndex(ro => ro.ResponseId);
            builder.HasIndex(ro => ro.OptionId);
        }
    }
}
