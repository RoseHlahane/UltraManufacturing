using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UltraManufacturing.Filters
{
    public class UMAuthenticationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        readonly List<string> _permissions;

        public UMAuthenticationAttribute(string permissions = "")
        {
            _permissions = permissions.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();
        }

        
    }
}
