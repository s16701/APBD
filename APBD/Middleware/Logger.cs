using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APBD.Middleware
{
    public class Logger
    {
        private readonly RequestDelegate request;

        public Logger(RequestDelegate req)
        {
            request = req;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (request != null)
            {
                string path = context.Request.Path;
                string protocol = context.Request.Protocol;
                string schema = context.Request.Scheme;
                string bodyStr = "";
                using(var reader= new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();

                    context.Request.Body.Position = 0;
                }
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "Logger.log")))
                {
                    await outputFile.WriteAsync(bodyStr);
                }
                await request(context);
            }

        }
    }
}
