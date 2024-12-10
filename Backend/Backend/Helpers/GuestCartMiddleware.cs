namespace Backend.Helpers
{
    public class GuestCartMiddleware
    {
        private readonly RequestDelegate _next;

        public GuestCartMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey("AuthToken") && !context.Request.Cookies.ContainsKey("guestCartId"))
            {
                var guestCartId = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                context.Response.Cookies.Append("guestCartId", guestCartId, cookieOptions);
            }

            await _next(context);
        }
    }
}
