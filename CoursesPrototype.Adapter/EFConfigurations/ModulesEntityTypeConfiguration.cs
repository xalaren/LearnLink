﻿using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesPrototype.Adapter.EFConfigurations
{
    public class ModulesEntityTypeConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.Property(module => module.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(module => module.Description)
                .HasMaxLength(500);
        }
    }
}