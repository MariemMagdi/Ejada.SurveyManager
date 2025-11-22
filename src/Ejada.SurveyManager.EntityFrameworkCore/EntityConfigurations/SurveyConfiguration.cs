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
    public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(Survey)));

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.Id)));

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(DomainConstants.SurveyNameMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.Name)));

            builder.Property(s => s.Purpose)
                .IsRequired(false)
                .HasMaxLength(DomainConstants.SurveyPurposeMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.Purpose)));

            builder.Property(s => s.TargetAudience)
                .IsRequired(false)
                .HasMaxLength(DomainConstants.SurveyTargetAudienceMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.TargetAudience)));

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.IsActive)));

            // ABP audit properties
            builder.Property(s => s.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.CreationTime)));

            builder.Property(s => s.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.CreatorId)));

            builder.Property(s => s.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.LastModificationTime)));

            builder.Property(s => s.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.LastModifierId)));

            builder.Property(s => s.IsDeleted)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.IsDeleted)));

            builder.Property(s => s.DeleterId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.DeleterId)));

            builder.Property(s => s.DeletionTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Survey.DeletionTime)));
        }
    }
}
