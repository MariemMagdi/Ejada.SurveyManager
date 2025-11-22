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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable(NamingHelper.ToPascalSnakeCase(nameof(Question)));

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.Id)));

            builder.Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(DomainConstants.QuestionTextMaxLength)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.Text)));

            builder.Property(q => q.Type)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.Type)));

            // ABP audit properties
            builder.Property(q => q.CreationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.CreationTime)));

            builder.Property(q => q.CreatorId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.CreatorId)));

            builder.Property(q => q.LastModificationTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.LastModificationTime)));

            builder.Property(q => q.LastModifierId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.LastModifierId)));

            builder.Property(q => q.IsDeleted)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.IsDeleted)));

            builder.Property(q => q.DeleterId)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.DeleterId)));

            builder.Property(q => q.DeletionTime)
                .HasColumnName(NamingHelper.ToPascalSnakeCase(nameof(Question.DeletionTime)));

            // Use field-backed collection
            builder.Navigation(q => q.Options).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(nameof(Question.Options)).UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
