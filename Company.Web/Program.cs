using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Services.Interfaces;
using Company.Services.Mapping;
using Company.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
          
           var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

  
            #region AddTransient
            //builder.Services.AddTransient<IDepartmentRepository , DepartmentRepository>();
            #endregion

            #region AddScoped
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    
            #endregion

            #region AddSingleton
            //builder.Services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
            #endregion

            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(x => x.AddProfile(new DepartmentProfile()));
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
			{
				config.Password.RequiredUniqueChars = 2;
				config.Password.RequireDigit = true;
				config.Password.RequireLowercase = true;
				config.Password.RequireUppercase = true;
				config.Password.RequiredLength = 6;
				config.Password.RequireNonAlphanumeric = true;

				config.User.RequireUniqueEmail = true;

				config.Lockout.AllowedForNewUsers = true;
				config.Lockout.MaxFailedAccessAttempts = 3;
				config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
			}
	         ).AddEntityFrameworkStores<CompanyDbContext>()
	         .AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.SlidingExpiration = true;
				options.LoginPath = "/Account/Login";
				options.LoginPath = "/Account/Logout";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.SameSite = SameSiteMode.Strict;

			});

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=SignUp}");

            app.Run();
        }
    }
}
