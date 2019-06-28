using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ms_userCrud.Models;

namespace ms_userCrud.Data.Mapping 
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Document)
                .IsRequired()
                .HasColumnName("Document");
            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnName("Name");
            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnName("Email");
            builder.Property(c => c.Password)
                .IsRequired()
                .HasColumnName("Passowrd");
        }
    }
}
