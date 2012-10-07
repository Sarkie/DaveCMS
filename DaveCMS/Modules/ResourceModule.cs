using Nancy;

namespace DaveCMS.Modules
{
    public class ResourceModule : NancyModule
    {
        public ResourceModule() : base("/products")
        {
            Get["/list"] =
                paramaters =>
                    {
                        return "The list of Products";
                    };
        }
    }
}