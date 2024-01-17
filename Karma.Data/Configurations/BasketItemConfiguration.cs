using System;
using Karma.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karma.Data.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder
     .HasOne(x => x.Basket)
     .WithMany(x => x.BasketItems)
     .HasForeignKey(x => x.BasketId)
     .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(x => !x.IsDeleted);


        }
    }
}

