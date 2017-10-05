﻿namespace ART.Domotica.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Http;

    using ART.Infra.CrossCutting.WebApi;

    [Authorize]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        #region Methods

        [Route("")]
        public IEnumerable<object> Get()
        {
            //string token = "";
            //Microsoft.Owin.Security.AuthenticationTicket ticket = Startup.OAuthBearerOptions.AccessTokenFormat.Unprotect(token);

            var identity = User.Identity as ClaimsIdentity;

            return identity.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        #endregion Methods
    }
}