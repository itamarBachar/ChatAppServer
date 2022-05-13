﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChatApp.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatAppContext") ?? throw new InvalidOperationException("Connection string 'ChatAppContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddCors(options => 
{
    // options.AddDefaultPolicy(policy => policy.AllowAnyOrigin());
    options.AddPolicy("cors_policy",
    builder =>
    {
        builder.WithOrigins("http://localhost:51042/",
                            "https://localhost:7243", "http://localhost:7243");
    });
});
var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}
//app.UseCors("Allow All");

//app.UseCors("cors_policy");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ratings}/{action=Index}/{id?}");


app.Run();
