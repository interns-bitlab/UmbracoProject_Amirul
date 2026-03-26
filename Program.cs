using Umbraco.Cms.Web.Common;
using MyCustomUmbracoProject.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<PublishContentDemo>();

builder.Services.AddScoped<itemServices>();

builder.Services.AddScoped<StudentServices>();

builder.Services.AddScoped<CourseServices>();       

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

WebApplication app = builder.Build(); 

app.UseSwagger();
app.UseSwaggerUI();

await app.BootUmbracoAsync();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
