using Ejada.SurveyManager.Common;
using Ejada.SurveyManager.EntityFrameworkCore.Helpers;
using Ejada.SurveyManager.Indicators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ejada.SurveyManager.EntityConfigurations
{
    public class IndicatorConfiguration : IEntityTypeConfiguration<Indicator>
    {
        public void Configure(EntityTypeBuilder<Indicator> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(Indicator)));

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.Id)));

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(DomainConstants.IndicatorNameMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.Name)));

            builder.Property(i => i.Description)
                .HasMaxLength(DomainConstants.IndicatorDescriptionMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.Description)));

            builder.Property(i => i.IsActive)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.IsActive)));

            // ABP audit properties
            builder.Property(i => i.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.CreationTime)));

            builder.Property(i => i.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.CreatorId)));

            builder.Property(i => i.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.LastModificationTime)));

            builder.Property(i => i.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.LastModifierId)));

            builder.Property(i => i.IsDeleted)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.IsDeleted)));

            builder.Property(i => i.DeleterId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.DeleterId)));

            builder.Property(i => i.DeletionTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Indicator.DeletionTime)));

            builder.HasIndex(i => i.Name);
        }
    }
}

