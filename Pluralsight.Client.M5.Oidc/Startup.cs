using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Pluralsight.Client.M5.Oidc
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();;

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false; // DEV only

                    options.ClientId = "oidc_client";
                    options.ClientSecret = "secret";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ResponseType = "code id_token";
                    options.Scope.Add("email");
                    options.Scope.Add("wiredbrain_api.rewards");
                    options.SaveTokens = true;

                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
