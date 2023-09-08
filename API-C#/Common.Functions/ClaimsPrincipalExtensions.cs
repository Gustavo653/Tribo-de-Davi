using System.Security.Claims;

namespace Common.Functions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string? GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string? GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static int? GetIdStudentTeacher(this ClaimsPrincipal user)
        {
            int? userId = null;
            var role = user.FindAll(ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            if (role != "Admin")
            {
                userId = Convert.ToInt32(user.GetUserId());
            }
            return userId;
        }
    }
}