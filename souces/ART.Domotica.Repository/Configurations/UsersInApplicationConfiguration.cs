﻿namespace ART.Domotica.Repository.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;

    using ART.Domotica.Repository.Entities;

    public class UsersInApplicationConfiguration : EntityTypeConfiguration<UsersInApplication>
    {
        #region Constructors

        public UsersInApplicationConfiguration()
        {
            //Primary Keys
            HasKey(x => new
            {
                x.UserId,
                x.ApplicationId,
            });

            //UserId
            Property(x => x.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            //ApplicationId
            Property(x => x.ApplicationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();

            //Foreing Keys
            HasRequired(x => x.ApplicationUser)
                .WithMany(x => x.UsersInApplication)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            //Foreing Keys
            HasRequired(x => x.Application)
                .WithMany(x => x.UsersInApplication)
                .HasForeignKey(x => x.ApplicationId)
                .WillCascadeOnDelete(false);
        }

        #endregion Constructors
    }
}