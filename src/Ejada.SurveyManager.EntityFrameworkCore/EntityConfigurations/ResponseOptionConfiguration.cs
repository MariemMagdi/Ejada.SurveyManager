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
    public class ResponseOptionConfiguration : IEntityTypeConfiguration<ResponseOption>
    {
        public void Configure(EntityTypeBuilder<ResponseOption> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption)));

            builder.HasKey(ro => ro.Id);

            builder.Property(ro => ro.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.Id)));

            builder.Property(ro => ro.ResponseId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.ResponseId)));

            builder.Property(ro => ro.OptionId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.OptionId)));

            // ABP audit properties
            builder.Property(ro => ro.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.CreationTime)));

            builder.Property(ro => ro.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.CreatorId)));

            builder.Property(ro => ro.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.LastModificationTime)));

            builder.Property(ro => ro.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(ResponseOption.LastModifierId)));

            builder.HasIndex(ro => ro.ResponseId);
            builder.HasIndex(ro => ro.OptionId);
        }
    }
}
