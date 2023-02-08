using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using PatientManagerService.Core;
using PatientManagerService.Infrastructure;
using PatientManagerService.Infrastructure.Data;
using PatientManagerService.Web;
using FastEndpoints;
using FastEndpoints.Swagger.Swashbuckle;
using FastEndpoints.ApiExplorer;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

/*
 * DB Options:
 * Configuration.GetConnectionString("DefaultConnection");
 * Configuration.GetConnectionString("SqliteConnection");
 */
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext(connectionString!);

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddRazorPages();
//TODO: uncomment if using fastendpoints
//builder.Services.AddFastEndpoints();
builder.Services.AddFastEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patient Manager Service API", Version = "v1" });
  c.EnableAnnotations();
  c.OperationFilter<FastEndpointsOperationFilter>();
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});

// form options for file upload size
builder.Services.Configure<FormOptions>(o =>
{
  o.ValueLengthLimit = int.MaxValue;
  o.MultipartBodyLengthLimit = 250000000; //TODO: Adjust via config - temp setting to 250MB
  o.MemoryBufferThreshold = int.MaxValue;
});

// TODO: CORS for dev purposes - define by domain for non dev
builder.Services.AddCors(options =>
{
  options.AddPolicy("CorsPolicy",
      builder => builder.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader());
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

//builder.Logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseCors("CorsPolicy");
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseRouting();

//TODO: uncomment if using fastendpoints
//app.UseFastEndpoints();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.MapDefaultControllerRoute();
app.MapRazorPages();

// Seed Database
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //                    context.Database.Migrate();
    context.Database.EnsureCreated();
    SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

app.Run();
