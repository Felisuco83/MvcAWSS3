using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAWSS3.Helpers
{
    public enum Folders
    {
        Images = 0,
        Documents = 1
    }
    public class PathProvider
    {
        IWebHostEnvironment environment;
        public PathProvider(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "images";
            }else if (folder == Folders.Documents)
            {
                carpeta = "documents";
            }
            string path = Path.Combine(this.environment.WebRootPath, carpeta, fileName);
            return path;
        }
    }
}
