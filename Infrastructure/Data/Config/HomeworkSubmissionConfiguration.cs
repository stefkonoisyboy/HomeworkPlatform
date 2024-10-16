using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class HomeworkSubmissionConfiguration : IEntityTypeConfiguration<HomeworkSubmission>
    {
        public void Configure(EntityTypeBuilder<HomeworkSubmission> builder)
        {
            builder
                .HasOne(hs => hs.Grade)
                .WithOne(g => g.HomeworkSubmission)
                .HasForeignKey<Grade>(g => g.HomeworkSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
