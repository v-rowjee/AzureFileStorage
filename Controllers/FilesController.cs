using FileStorage.Services;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace FileStorage.Controllers
{
    public class FilesController : Controller
    {
        IAzureFileStorage AzureFileStorage;
        public FilesController(IAzureFileStorage AzureFileStorage)
        {
            this.AzureFileStorage = AzureFileStorage;
        }
        public FilesController()
        {
            this.AzureFileStorage = new AzureFileStorage();
        }


        // GET: Files
        public ActionResult Index()
        {
            return RedirectToAction("Dir", new { id = "dir" });
        }

        // GET: Files/Dir/xx
        public ActionResult Dir(string id)
        {
            if(id == null)
            {
                return RedirectToAction("Dir", new { id = "dir" });
            }

            AzureFileStorage.Init(id);

            ViewBag.Files = AzureFileStorage.ViewFiles();

            ViewBag.Directories = AzureFileStorage.GetDirectories();

            ViewBag.CurrentDir = id;

            return View();
        }

        // POST: Files/Download
        [HttpPost]
        public ActionResult Download(string dir, string name)
        {
            var dirName = dir;
            var fileName = name;

            MemoryStream ms = new MemoryStream();

            try
            {
                AzureFileStorage.Init(dirName);

                var file = AzureFileStorage.DownloadFile(fileName);

                // Write the contents of the file to the console window.
                file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;

                return File(blobStream, file.Properties.ContentType, file.Name);
            }
            catch (Exception e)
            {
                TempData["ErrorDownload"] = e.Message;
                return RedirectToAction("Index");
            }
        }


        // POST: Files/Delete
        [HttpPost]
        public ActionResult Delete(string dir, string name)
        {
            AzureFileStorage.Init(dir);

            var fileDeleted = AzureFileStorage.ViewFiles()
                .FirstOrDefault(m => m.Name.Equals(name));

            var result = AzureFileStorage.DeleteFile(name);

            if (result)
            {
                TempData["SuccessDelete"] = fileDeleted.Name;
            }
            else
            {
                TempData["ErrorDelete"] = fileDeleted.Name;
            }
            return RedirectToAction("Dir", new { id = dir });
        }


    }
}