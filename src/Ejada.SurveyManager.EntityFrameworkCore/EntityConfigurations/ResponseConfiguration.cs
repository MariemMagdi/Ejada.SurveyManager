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
    public class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.ToTable("Responses");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.SurveyInstanceId).IsRequired();

            builder.Property(r => r.QuestionId).IsRequired();

            builder.Property(r => r.AnswerValue).IsRequired(false);

            builder.HasIndex(r => r.SurveyInstanceId);
            builder.HasIndex(r => r.QuestionId);
        }
    }
}
