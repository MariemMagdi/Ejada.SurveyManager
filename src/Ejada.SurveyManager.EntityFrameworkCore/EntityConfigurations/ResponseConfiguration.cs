using Ejada.SurveyManager.EntityFrameworkCore.Helpers;
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
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(Response)));

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.Id)));

            builder.Property(r => r.SurveyInstanceId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.SurveyInstanceId)));

            builder.Property(r => r.QuestionId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.QuestionId)));

            builder.Property(r => r.AnswerValue)
                .IsRequired(false)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.AnswerValue)));

            // ABP audit properties
            builder.Property(r => r.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.CreationTime)));

            builder.Property(r => r.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.CreatorId)));

            builder.Property(r => r.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.LastModificationTime)));

            builder.Property(r => r.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Response.LastModifierId)));

            builder.HasIndex(r => r.SurveyInstanceId);
            builder.HasIndex(r => r.QuestionId);
        }
    }
}
