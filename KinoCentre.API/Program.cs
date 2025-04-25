using KinoCentre.DB;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "KinoCentre.API", Version = "v1" });
});

builder.Services.AddDbContext<KinoDbContext>();

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddControllers().AddNewtonsoftJson( options => 
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

builder.Services.AddRouting();


var app = builder.Build();

app.UseCors("AllowAll");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "WTF is this");
});

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Coursery API v1");
    options.RoutePrefix = string.Empty;
});

app.Run();

