using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Models;

namespace Project.DAL.Data.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn();
            builder.Property(D => D.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
        }
    }
}
