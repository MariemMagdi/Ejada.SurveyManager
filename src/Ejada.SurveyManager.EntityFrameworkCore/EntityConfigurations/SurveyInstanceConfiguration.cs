using Ejada.SurveyManager.EntityFrameworkCore.Helpers;
using Ejada.SurveyManager.SurveyInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ejada.SurveyManager.EntityConfigurations
{
    public class SurveyInstanceConfiguration : IEntityTypeConfiguration<SurveyInstance>
    {
        public void Configure(EntityTypeBuilder<SurveyInstance> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance)));

            builder.HasKey(si => si.Id);

            builder.Property(si => si.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.Id)));

            builder.Property(si => si.SurveyId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.SurveyId)));

            builder.Property(si => si.AssigneeUserId)
                .IsRequired()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.AssigneeUserId)));

            builder.Property(si => si.DueDate)
                .IsRequired(false)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.DueDate)));

            builder.Property(si => si.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.Status)));

            // ABP audit properties
            builder.Property(si => si.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.CreationTime)));

            builder.Property(si => si.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.CreatorId)));

            builder.Property(si => si.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.LastModificationTime)));

            builder.Property(si => si.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.LastModifierId)));

            builder.Property(si => si.IsDeleted)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.IsDeleted)));

            builder.Property(si => si.DeleterId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.DeleterId)));

            builder.Property(si => si.DeletionTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(SurveyInstance.DeletionTime)));

            builder.HasIndex(si => si.SurveyId);
            builder.HasIndex(si => si.AssigneeUserId);

        }
    }
}
