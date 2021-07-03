using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using CompanyProjectManagementSystem.DBConnection;
using CompanyProjectManagementSystem.Authentication;
using CompanyProjectManagementSystem.Models;
using CompanyProjectManagementSystem.Filters;

namespace CompanyProjectManagementSystem.Controllers
{
    public class ProjectController : ApiController
    {
        DataBase db = new DataBase();


        [Route("api/projects")]
        [HttpGet]
        [JwtAuthorize]
        public HttpResponseMessage Projects(int pagenumber,int pagesize)
        {
            try
            {
                string queryCount = "EXEC USP_GETPROJECTS 0,0,1";
                DataTable dtCount= db.GetData(queryCount);
                int count=Convert.ToInt32(dtCount.Rows[0][0]);

                int skip = (pagenumber - 1) * pagesize;
                int take = pagesize;

                int TotalPages = (int)Math.Ceiling(count / (double)pagesize);

                string query = "EXEC USP_GETPROJECTS " + skip+","+take+"";
                DataTable dt = db.GetData(query);
                var response = new {
                    result = new {
                        data = dt,
                        page = pagenumber,
                        pageSize = pagesize,
                        hasMore= pagenumber < TotalPages ? "Yes" : "No",
                        totalItems=count
                    }
            };
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("api/login")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody] Login login)
        {
            try
            {
                string token;
                DataTable dt = db.GetData(@"SELECT * FROM EMPLOYEEMASTER WHERE EMAIL='"+login.EMAIL+"' AND PASSWORD='"+ login.PASSWORD + "'");
                if (dt.Rows.Count >= 1)
                {
                    token=JwtManager.GenerateToken(login.EMAIL);
                    var response = new
                    {
                        TOKEN = token,
                        EMAIL = login.EMAIL
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,new {message="User Email or Password is Incorrect" });
                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
