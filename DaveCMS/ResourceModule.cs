using Nancy;

namespace DaveCMS
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