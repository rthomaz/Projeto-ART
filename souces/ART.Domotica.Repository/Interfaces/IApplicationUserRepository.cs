﻿namespace ART.Domotica.Repository.Interfaces
{
    using System;

    using ART.Domotica.Repository.Entities;
    using ART.Infra.CrossCutting.Repository;

    public interface IApplicationUserRepository : IRepository<ARTDbContext, ApplicationUser, Guid>
    {
    }
}