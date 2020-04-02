using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHost
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //------------------------------------------------------------------------
            // Configure Web API for self-host application 
            //------------------------------------------------------------------------
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}", // requests under the api sub-url forwarded to WebAPI controllers
                defaults: new { id = RouteParameter.Optional }
            );

            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
                .EnableSwaggerUi();

            app.UseWebApi(config);

            //------------------------------------------------------------------------
            // File server 1: serve the directory provided with the relative path
            //------------------------------------------------------------------------
            if (!Directory.Exists("wwwroot")) Directory.CreateDirectory("wwwroot");

            var physicalFileSystem = new PhysicalFileSystem("./wwwroot");

            // file server options
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem, // register file system
                EnableDirectoryBrowsing = false
            };

            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            //app.Use<DefaultFileRewriterMiddleware>(physicalFileSystem);  // middleware to direct non-existing file URLs to index.html
            app.UseFileServer(options);

            //------------------------------------------------------------------------
            // File server 2: serve another directory, defined with the abslute path
            //------------------------------------------------------------------------
            if (!Directory.Exists(@"C:\SelfHost\wwwroot")) Directory.CreateDirectory(@"C:\SelfHost\wwwroot");

            // copy staticPage contents to dingle page at the destination dir @"C:\SelfHost\wwwroot\index.html"
            File.WriteAllText(@"C:\SelfHost\wwwroot\index.html", this.staticPage);

            var physicalFileSystem1 = new PhysicalFileSystem(@"C:\SelfHost\wwwroot");

            // file server options
            var options1 = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem1, // register another file system
                EnableDirectoryBrowsing = false,
                RequestPath = new Microsoft.Owin.PathString("/absolute")
            };
            //app.Use<DefaultFileRewriterMiddleware>(physicalFileSystem1);
            app.UseFileServer(options1);

        }

        private string staticPage =
        @"<html>
          <head><title> OWIN Self-Hosting</title></head>
          <body bgcolor = ""#E6FAFA"" >
          <p>Owin self-hosting web-server is serving the C:\SelfHost\wwwroot directory.</p>
	      <p><a href = ""http://localhost:9090"" > localhost:9090</a></p>
          </body>
          </html>";

    }
}
