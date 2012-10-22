using Nancy;

namespace DaveCMS.Modules
{
    public class ExampleSiteModule : NancyModule
    {
        public ExampleSiteModule()
        {

            Get["/"] = parameters =>
                           {
                               return View["HelloWorld.html"];
                           };
        } 
    }
}