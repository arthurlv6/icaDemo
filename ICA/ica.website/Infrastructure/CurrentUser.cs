using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ica.website.Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity _identity;
        private readonly ApplicationDbContext _context;

        private ApplicationUser _user;

        public CurrentUser(IIdentity identity, ApplicationDbContext context)
        {
            _identity = identity;
            _context = context;
        }

        public Models.ApplicationUser User
        {
            get
            {
                return _user ?? (_user = _context.Users.Find(_identity.GetUserId()));
            }
        }
    }
}