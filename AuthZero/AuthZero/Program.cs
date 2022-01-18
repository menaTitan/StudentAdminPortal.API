using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
ConfigurationManager configuration = builder.Configuration;

// these are  part of ASP.NET Core authentication package and registering the default settings which we will override with AuthZero 
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie().AddOpenIdConnect("Auth0", opt =>
{
    //Set the authority to the Auth0 domain 
    opt.Authority = $"https://{configuration["AuthO:Domain"]}";
    //Set the authority to the Auth0 client id 
    opt.ClientId = configuration["AuthO:ClientId"];
    //Set the authority to the ClinetSecret
    opt.ClientSecret = configuration["AuthO:ClientSecret"];
    //Set response type code you going to get from Auth0
    opt.ResponseType = OpenIdConnectResponseType.Code;

    //set scope 
    opt.Scope.Clear();

    opt.Scope.Add("openId");

    //set the callback path
    opt.CallbackPath = new PathString("/callback");

    //set issuer to indicate that we are using Auth0
    opt.ClaimsIssuer = "Auth0";

    opt.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
    {
        // handle the logout redirection
        OnRedirectToIdentityProviderForSignOut = (context) =>
        {
            var logoutUri = $"https://{configuration["Auth0:Domain"]}/v2/logout?client_id={configuration["Auth0:ClientId"]}";

            var postLogoutUri = context.Properties.RedirectUri;
            if (!string.IsNullOrEmpty(postLogoutUri))
            {
                if (postLogoutUri.StartsWith("/"))
                {
                    // transform to absolute
                    var request = context.Request;
                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                }
                logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
            }

            context.Response.Redirect(logoutUri);
            context.HandleResponse();

            return Task.CompletedTask;
        }

    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
