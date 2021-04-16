using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAWSS3.Helpers;
using MvcAWSS3.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAWSS3.Controllers
{
    public class AWSFilesController : Controller
    {
        private UploadHelper uploadhelper;
        public ServiceAWSS3 ServiceS3;

        public AWSFilesController (UploadHelper uploadhelper, ServiceAWSS3 service)
        {
            this.uploadhelper = uploadhelper;
            this.ServiceS3 = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListFilesAWS()
        {
            List<string> files = await this.ServiceS3.GetS3FilesAsync();
            return View(files);
        }

        public IActionResult UploadFileAWS()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFileAWS(IFormFile file)
        {
            string path = await this.uploadhelper.UploadFileAsync(file, Folders.Images);
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                bool respuesta = await this.ServiceS3.UploadFileAsync(stream, file.FileName);
                ViewBag.Mensaje = "Archivo en AWS Bucket: " + respuesta;
            };
            return View();
            //bool respuesta = await this.ServiceS3.UploadFileAsync
        }

        public async Task<IActionResult> FileAWS (string fileName)
        {
            Stream stream = await this.ServiceS3.GetFileAsync(fileName);
            return File(stream, "image/png");
        }

        public async Task<IActionResult> DeleteFileAWS(string fileName)
        {
            await this.ServiceS3.DeleteFileAsync(fileName);
            return RedirectToAction("ListFilesAWS");
        }
    }
}
