using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static TCCVetCare.Models.AuthenticationModel;

namespace TCCVetCare.Models
{
    public class AuthenticationAuthorizeAttribute : AuthorizeAttribute
    {
        // [CustomeAuthorize(UserRole.Admin)]

        private readonly UserRole roleAutorizada;

        public AuthenticationAuthorizeAttribute(UserRole role)
        {
            roleAutorizada = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Verificar se o usuário está autenticado
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            // Verificar se o usuário tem a role autorizada
            var userRole = httpContext.User.Identity.Name;

            // Verificar se o usuário é admin ou tem a role autorizada
            if (userRole == UserRole.Admin.ToString() && userRole == roleAutorizada.ToString())
                return true;

            return false;
        }
    }
}