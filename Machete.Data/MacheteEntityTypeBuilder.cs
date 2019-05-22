using System;
using Machete.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Machete.Data
{
    public static class MacheteEntityTypeBuilder
    {
        public static EntityTypeBuilder<T> Configurations<T>(this ModelBuilder model) where T : Record
        {
            return model.Entity<T>();
        }

        public static void Add(this EntityTypeBuilder entity, Type type)
        {
            Activator.CreateInstance(type, entity);
        }
    }
}