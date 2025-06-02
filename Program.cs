using Microsoft.EntityFrameworkCore;
using RecipeApi.Data;
using System.Threading.RateLimiting;
//using RecipeApi.Services.Interfaces;
//using RecipeApi.Services.Interfaces;
//using RecipeApi.Services;

var builder = WebApplication.CreateBuilder(args);


//rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
    
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

//controller va views
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Swagger/OpenAPI de test API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DbContext voi SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//CORS policy
//bypass same origin restriction, enabling web pages to interact with resources on different domains. 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",    //allow all origins, methods, and headers. might need to be more restrictive in later patches
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Them response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

//builder.Services.AddAutoMapper(typeof(Program));
///////
//builder.Services.AddScoped<IRecipeService, RecipeService>();

///////



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Use rate limiting
app.UseRateLimiter();

// Su dung response compression
app.UseResponseCompression();

app.UseHttpsRedirection();

// Su dung CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
