﻿namespace ART.Infra.CrossCutting.WebApi
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    public abstract class AuthenticatedApiController : BaseApiController
    {
        #region Properties

        protected Guid _userId
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                var claim = identity.Claims.First(x => x.Type == "userId");
                var userId = Guid.Parse(claim.Value);
                return userId;
            }
        }

        #endregion Properties
    }
}