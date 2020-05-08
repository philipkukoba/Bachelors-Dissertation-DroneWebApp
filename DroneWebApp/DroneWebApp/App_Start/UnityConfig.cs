using DroneWebApp.Controllers;
using DroneWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
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
            container.RegisterType<DbContext, DroneDBEntities > ("DroneDBEntities"); // added

            container.RegisterType<IdentityDbContext<ApplicationUser>, ApplicationDbContext>("ApplicationDbContext", new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            container.RegisterType<DroneFlightsController>(new InjectionConstructor(new ResolvedParameter<DbContext>("DroneDBEntities")));
            container.RegisterType<DronesController>(new InjectionConstructor(new ResolvedParameter<DbContext>("DroneDBEntities")));
            container.RegisterType<FilesController>(new InjectionConstructor(new ResolvedParameter<DbContext>("DroneDBEntities")));
            container.RegisterType<MapController>(new InjectionConstructor(new ResolvedParameter<DbContext>("DroneDBEntities")));
            container.RegisterType<PilotsController>(new InjectionConstructor(new ResolvedParameter<DbContext>("DroneDBEntities")));
            container.RegisterType<ProjectsController>(new InjectionConstructor(new ResolvedParameter<DbContext>("DroneDBEntities")));
            //container.RegisterType<AccountController>(new InjectionConstructor(new ResolvedParameter<DbContext>("ApplicationDbContext")));
            //container.RegisterType<ManageController>(new InjectionConstructor(new ResolvedParameter<DbContext>("ApplicationDbContext")));

            DroneFlightsController droneFlightsController = container.Resolve<DroneFlightsController>();
            DronesController dronesController = container.Resolve<DronesController>();
            FilesController filesController = container.Resolve<FilesController>();
            MapController mapController = container.Resolve<MapController>();
            PilotsController pilotsController = container.Resolve<PilotsController>();
            ProjectsController projectsController = container.Resolve<ProjectsController>();
            AccountController accountController = container.Resolve<AccountController>();
            ManageController manageController = container.Resolve<ManageController>();
        }
    }
}