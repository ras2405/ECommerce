using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using LogicInterfaces;
using Domain;

namespace ECommerceApi.Filters
{
    public class AuthenticationFilter : Attribute, IActionFilter
    {
        public string RoleNeeded { get; set; }
        public bool EditUser { get; set; } = false;

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            string header = context.HttpContext.Request.Headers["Authorization"];
            Guid token = Guid.Empty;
            
            if (header != null)
            {
                header = header.Replace("Bearer ", "");
            }
            
            if (checkHeader(header, ref token))
            {
                context.Result = new ObjectResult("Authorization header is required.")
                {
                    StatusCode = 401
                };
                return;
            }
            else 
            {
                ISessionLogic _sessionLogic = GetSessionLogic(context);
                User currentUser = _sessionLogic.GetCurrentUser(token);
                if (currentUser == null)
                {
                    context.Result = new ObjectResult(new { Message = "User is not logged in" })
                    {
                        StatusCode = 401
                    };
                }
                else 
                {
                    if (EditUser)
                    {
                        string routeIdString = context.RouteData.Values["id"]?.ToString();
                        if (Guid.TryParse(routeIdString, out var routeId) && (currentUser.Id == routeId))
                        {
                            RoleNeeded = null;
                        }
                    }

                    if (RoleNeeded != null && currentUser.Rol != StringToRoles(RoleNeeded) && currentUser.Rol != User.Roles.Both)
                    {
                        context.Result = new ObjectResult(new { Message = "Action is not available for current user" })
                        {
                            StatusCode = 403
                        };
                    }
                }
            }
        }

        private User.Roles StringToRoles(string roleNeeded)
        {
            if (roleNeeded == "Buyer")
            {
                return User.Roles.Buyer;
            }
            return User.Roles.Admin;
        }

        protected ISessionLogic GetSessionLogic(ActionExecutingContext context)
        {
            Object sessionManagerObject = context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
            ISessionLogic sessionService = sessionManagerObject as ISessionLogic;

            return sessionService;
        }

        private static bool checkHeader(string header, ref Guid token)
        {
            return string.IsNullOrEmpty(header) || !Guid.TryParse(header, out token);
        }
    }
}
