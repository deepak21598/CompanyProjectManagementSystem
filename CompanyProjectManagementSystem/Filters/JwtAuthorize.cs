using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using CompanyProjectManagementSystem.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using CompanyProjectManagementSystem.DBConnection;
using System.Data;

namespace CompanyProjectManagementSystem.Filters
{
    public class JwtAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            return;
        }

        private void Unauthorized()
        {
            throw new NotImplementedException();
        }

        private bool AuthorizeRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return false;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                return false;
            }

            var token = authorization.Parameter;
            string tokenUsername = JwtManager.ValidateToken(token);
            DataBase db = new DataBase();
            DataTable dt=db.GetData("SELECT * FROM EMPLOYEEMASTER WHERE EMAIL='" + tokenUsername + "'");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}