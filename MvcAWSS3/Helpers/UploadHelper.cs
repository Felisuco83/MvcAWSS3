using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAWSS3.Helpers
{
    public class UploadHelper
    {
        private PathProvider provider;
        public UploadHelper(PathProvider provider)
        {
            this.provider = provider;
        }

        public async Task<string> UploadFileAsync (IFormFile formFile, Folders folder)
        {
            string fileName = formFile.FileName;
            string path = this.provider.MapPath(fileName, Folders.Images);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            };
            return path;
        }
    }
}
