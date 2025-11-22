using Ejada.SurveyManager.Common;
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
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(Option)));

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.Id)));

            builder.Property(o => o.QuestionId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.QuestionId)));

            builder.Property(o => o.Label)
                .IsRequired()
                .HasMaxLength(DomainConstants.OptionLabelMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.Label)));

            builder.Property(o => o.Type)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.Type)));

            // ABP audit properties
            builder.Property(o => o.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.CreationTime)));

            builder.Property(o => o.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.CreatorId)));

            builder.Property(o => o.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.LastModificationTime)));

            builder.Property(o => o.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.LastModifierId)));

            builder.Property(o => o.IsDeleted)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.IsDeleted)));

            builder.Property(o => o.DeleterId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.DeleterId)));

            builder.Property(o => o.DeletionTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Option.DeletionTime)));

            builder.HasIndex(o => o.QuestionId);

            builder.HasOne<Question>()
                .WithMany(q => q.Options)
                .HasForeignKey(o=>o.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
