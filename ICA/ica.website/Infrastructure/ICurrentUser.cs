using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ica.website.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}