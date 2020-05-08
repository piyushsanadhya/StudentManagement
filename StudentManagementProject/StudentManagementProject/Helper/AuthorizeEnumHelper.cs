using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Helper
{
    public class AuthorizeEnumHelper : AuthorizeAttribute
    {
        /// <summary>
        /// you can enter enums in the authorize attribute
        /// </summary>
        /// <param name="roles"></param>

        public AuthorizeEnumHelper(params object[] roles)
        {
            if (!roles.Any(r => r.GetType().IsEnum))
            {
                throw new ArgumentException("roles");
            }

            this.Roles = string.Join(",", roles.Select(r => r.ToString()));
        }
    }
}
