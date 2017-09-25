﻿using ART.Domotica.DistributedServices.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ART.Domotica.DistributedServices.Configurations
{
    public class UserInSpaceConfiguration : EntityTypeConfiguration<UserInSpace>
    {
        public UserInSpaceConfiguration()
        {
            //Primary Keys
            HasKey(x => new
            {
                x.UserId,
                x.SpaceId,
            });

            //Foreing Keys            
            HasRequired(x => x.User)
                .WithMany(x => x.UsersInSpace)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            //Foreing Keys            
            HasRequired(x => x.Space)
                .WithMany(x => x.UsersInSpace)
                .HasForeignKey(x => x.SpaceId)
                .WillCascadeOnDelete(false);
        }
    }
}