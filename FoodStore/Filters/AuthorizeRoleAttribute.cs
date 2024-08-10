using FoodStore.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Filters
{
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeRoleAttribute(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var userService = httpContext.RequestServices.GetService(typeof(UserService)) as UserService;

            var userId = httpContext.Session.GetString("UserId");
            if (userId == null)
            {
                context.Result = new RedirectResult("/Account/Login");
                return;
            }

            var roles = userService.GetUserRoles(userId);
            if (!roles.Contains(_role))
            {
                context.Result = new RedirectToActionResult("Error", "Home", null);
            }
        }
    }
}
