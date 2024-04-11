using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Utility;
using ShoppingCartApi.Extensions;
using Mango.Services.Business.Interfaces;
using Mango.Services.Business;
using Mango.Services.Data;
using Mango.Services.CouponAPI.Interfaces;
using Mango.Services.ShoppingCartAPI.Service;
using Mango.Services.ProductAPI.Service;
using Mango.Services.ProductAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
       .AddEndpoints()
       .AddSwaggerDefinition()
       .AddDbContext<DataContext>(options =>
       {
           options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
       })
       .AddCors(options =>
       {
           options.AddDefaultPolicy(configurePolicy =>
           {
               configurePolicy
                              .WithOrigins()
                              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                              .AllowAnyHeader()
                              .AllowCredentials()
                              .WithExposedHeaders(HeaderNames.AccessControlAllowOrigin,
                                                  HeaderNames.AccessControlAllowCredentials,
                                                  HeaderNames.AccessControlRequestHeaders,
                                                  HeaderNames.AccessControlRequestMethod,
                                                  HeaderNames.ContentDisposition);
           });
       });

builder.Services.AddEndpointsApiExplorer();


builder.AddAppAuthetication();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddTransient<IShoppingCartBusiness, ShoppingCartBusiness>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICouponService, CouponService>();

//EXTERNAL SERVICES
builder.Services.AddHttpClient("Product", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

var app = builder.Build();

//Endpoints
app.UseApplicationEndpoints();

// Configure the HTTP request pipeline.
//Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Cart API");
        c.RoutePrefix = string.Empty;
    }
});

//Exceptions
app.UseHttpsRedirection();
app.UseCors();

//Auth
app.UseAuthentication();
app.UseAuthorization();

app.Run();