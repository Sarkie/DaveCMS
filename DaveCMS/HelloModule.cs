using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace DaveCMS
{
    public class HelloModule : NancyModule
    {
        private readonly IMessageService _messageService;

        public HelloModule(IMessageService messageService)
        {
            _messageService = messageService;

            if(_messageService==null)
                throw new ArgumentNullException("messageService");

            Get["/"] = paramaters => "Hello World";

            // would capture routes like /hello/nancy sent as a GET request
//            Get["/hello/{name}"] = parameters =>
//            {
//                return "Hello " + parameters.name;
//            };

            // would capture routes like /products/1034 sent as a DELETE request
            Delete[@"/products/(?<id>[\d]{1,7})"] = parameters =>
            {
                return 200;
            };

            // would capture routes like /users/192/add/moderator sent as a POST request
            Post["/users/{id}/add/{category}"] = parameters =>
            {
                return HttpStatusCode.OK;
            };

            Get["/hello/{name}"] = parameters =>
                                       {
                                           this.Context.Trace.TraceLog.WriteLog(s => s.AppendLine("Root Path"));

                                           var f = this.Bind<Person>();

                                           //this.Response.FromStream()
                                           return "Hello " + f.Name;
                                       };

            Get["/goodbye/{name}"] = parameters =>
            {
                return "Goodbye " + parameters["name"];
            };

            Get["/webpage"] = paramaters =>
                                  {
                                      return View["HelloWorld.html", new Person()];
                                  };

            Before += ctx => {
                                 return null;
            };

            After += ctx =>
                         {
                             //ctx.Response.ContentType = "text/plain";
                         };

            Post["/login/"] = paramaters =>
                                  {
                                      return new RedirectResponse("/login?error=true&username=username",
                                                                  RedirectResponse.RedirectType.SeeOther);

                                  };
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}