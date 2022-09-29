using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Confiig;

public class BaseEntityConfigurations<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        //CreatedDate 
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
        //Updated Date
        builder.Property(x => x.ModifiedAt).HasDefaultValueSql("GETDATE()");
    }
}
