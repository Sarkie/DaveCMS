using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
using Raven.Client;
using TinyIoC;

namespace DaveCMS
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
//        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
//        {
//            base.ApplicationStartup(container, pipelines);
//
//            this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
//            {
//                return string.Concat("Views/", viewName);
//            });
//
//        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                //This will tell Nancy it won't have to look in the Nhibernate or Lucene assemblies for implementations of your
                //interfaces.
                return NancyInternalConfiguration
                    .Default
                    .WithIgnoredAssembly(asm => asm.FullName.StartsWith("NHibernate", StringComparison.InvariantCulture))
                    .WithIgnoredAssembly(asm => asm.FullName.StartsWith("Lucene", StringComparison.InvariantCulture));
            }
        }

        //        protected override Type RootPathProvider
        //        {
        //            get { return typeof(CustomRootPathProvider); }
        //        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"123" }; }
        }
    }
}