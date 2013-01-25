using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using KinHelper.Model.Db;

namespace KinHelper.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureDatabase();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            // controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerHttpRequest();
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // http base classes
            //builder.RegisterModule(new AutofacWebTypesModule());
            // register database
            builder.RegisterType<KinHelperContext>().InstancePerHttpRequest();

            builder.RegisterType<ExtensibleActionInvoker>().As<IActionInvoker>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void ConfigureDatabase()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<KinHelperContext>());
        }
    }
}