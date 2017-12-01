﻿namespace ART.Domotica.Repository.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using ART.Domotica.Repository.Entities;

    public class UnitMeasurementScaleConfiguration : EntityTypeConfiguration<UnitMeasurementScale>
    {
        #region Constructors

        public UnitMeasurementScaleConfiguration()
        {
            ToTable("UnitMeasurementScale", "SI");

            //Primary Keys
            HasKey(x => new
            {
                x.NumericalScalePrefixId,
                x.NumericalScaleTypeId,
            });

            //NumericalScalePrefixId
            Property(x => x.NumericalScalePrefixId)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();

            //NumericalScalePrefix
            HasRequired(x => x.NumericalScalePrefix)
                .WithMany(x => x.UnitMeasurementScales)
                .HasForeignKey(x => x.NumericalScalePrefixId)
                .WillCascadeOnDelete(false);

            //NumericalScaleTypeId
            Property(x => x.NumericalScaleTypeId)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();

            //NumericalScaleType
            HasRequired(x => x.NumericalScaleType)
                .WithMany(x => x.UnitMeasurementScales)
                .HasForeignKey(x => x.NumericalScaleTypeId)
                .WillCascadeOnDelete(false);

            //Name
            Property(x => x.Name)
                .HasColumnOrder(2)
                .HasMaxLength(30)
                .IsRequired();

            //Base
            Property(x => x.Base)
                .HasColumnOrder(3)
                .IsRequired();

            //Exponent
            Property(x => x.Exponent)
                .HasColumnOrder(4)
                .IsRequired();
        }

        #endregion Constructors
    }
}