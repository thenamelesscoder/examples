using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;

namespace SimpleBearerAuth
{
  public class Startup
  {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Startup(IWebHostEnvironment webHostEnvironment)
    {
      _webHostEnvironment = webHostEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDataProtection()
        .SetApplicationName("SimpleBearerAuth")
        .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(_webHostEnvironment.ContentRootPath, "Keys")));

      services
        .AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = "Jwt";
          options.DefaultChallengeScheme = "Jwt";
        })
        .AddJwtBearer("Jwt", options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySymmetricSecurityKey")),
            ValidateLifetime = true,
          };
        });

      services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
