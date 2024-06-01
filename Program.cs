using System;
using System.Linq;
using MyRHApp.Models;
using MyRHApp.Services;
using MyRHApp.Services.Interfaces;
using MyRHApp.Utilities;
using MyRHApp.DataAccess.Interfaces;
using MyRHApp.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MyRHApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var consoleManager = new ConsoleManager(
                serviceProvider.GetService<IUserService>(),
                serviceProvider.GetService<IEmployeeService>(),
                serviceProvider.GetService<IRoleService>()
            );

            consoleManager.Start();
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IUserRoleContextRepository, UserRoleContextRepository>()
                .AddSingleton<IRoleRepository, RoleRepository>()
                .AddSingleton<IEmployeeRepository, EmployeeRepository>()
                .AddSingleton<IContextRepository, ContextRepository>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IRoleService, RoleService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .BuildServiceProvider();
        }
    }
}
