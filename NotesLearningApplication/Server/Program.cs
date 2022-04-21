using Microsoft.AspNetCore.ResponseCompression;
using DatabaseServices;
using Microsoft.EntityFrameworkCore;
using DatabaseServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using NotesLearningApplication.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NotesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NotesDatabase")));
builder.Services.AddScoped<NotesDbSeeder>();

builder.Services.AddScoped<ICategoriesDbService, CategoriesDbService>();
builder.Services.AddScoped<IUsersDbService, UsersDbService>();
builder.Services.AddScoped<INotesDbService, NotesDbService>();


builder.Services.AddScoped<ErrorHandlingMiddleware, ErrorHandlingMiddleware>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
if(builder.Environment.IsDevelopment()){
    builder.Services.AddSwaggerGen(options => {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
            Description = "Standard yo",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SECRET:SecurityKey").Value)),
            ValidateIssuer = false,
            ValidateAudience=false
        };
    });

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
    //Seed database
    app.Services.CreateScope().ServiceProvider.GetService<NotesDbSeeder>()?.Seed();
    app.UseSwagger();
    app.UseSwaggerUI();
} else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
