using DroneWebApp.Models;
using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace DroneWebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            // Register the database and let Unity manage it for Dependency Injection
            container.RegisterType<DbContext, DroneDBEntities > (); // added
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}