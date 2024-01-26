using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using TestApp.Domain.Models.Options;

namespace TestApp.Application.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ServiceOptions _requestProvider;

        public AuthMiddleware(RequestDelegate next, IOptions<ServiceOptions> requestOptions)
        {
            _next = next;
            _requestProvider = requestOptions.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!AuthorizeRequest(context.Request, username: _requestProvider.Username, password: _requestProvider.Password))
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                await _next.Invoke(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                return;
            }
        }

        public bool AuthorizeRequest(HttpRequest request, string username, string password)
        {
            bool result = false;

                string path = request.Path.Value.ToLower();

                if (path.Contains("docs") || path.Contains("swagger"))
                {
                    result = true;
                }
                else
                {
                    //service
                    if (request.Headers["Authorization"].ToString().ToUpper().StartsWith("BASIC"))
                    {
                        string usernamePassword = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(request.Headers["Authorization"].ToString().Substring("Basic ".Length).Trim()));

                        var user = usernamePassword.Substring(0, usernamePassword.IndexOf(':'));
                        var pass = usernamePassword.Substring(usernamePassword.IndexOf(':') + 1);

                        if (user == username && pass == password)
                            result = true;
                    }
                }

            return result;
        }
    }
}
