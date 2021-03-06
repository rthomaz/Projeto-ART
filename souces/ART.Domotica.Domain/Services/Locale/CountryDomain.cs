﻿using System.Threading.Tasks;
using System.Collections.Generic;
using ART.Infra.CrossCutting.Domain;
using ART.Domotica.Domain.Interfaces.Locale;
using ART.Domotica.Repository.Entities.Locale;
using ART.Domotica.Repository.Interfaces.Locale;
using Autofac;
using ART.Domotica.Repository;
using ART.Domotica.Repository.Repositories.Locale;

namespace ART.Domotica.Domain.Services.Locale
{
    public class CountryDomain : DomainBase, ICountryDomain
    {
        #region private readonly fields

        private readonly ICountryRepository _countryRepository;

        #endregion

        #region constructors

        public CountryDomain(IComponentContext componentContext)
        {
            var context = componentContext.Resolve<ARTDbContext>();

            _countryRepository = new CountryRepository(context);
        }

        #endregion

        #region public voids

        public async Task<List<Country>> GetAll()
        {
            return await _countryRepository.GetAll();
        }

        #endregion
    }
}
