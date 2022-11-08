﻿using FileStorage.Services;
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
        // GET: Files
        public ActionResult Index()
        {
            var files = AzureFileStorage.ViewFiles();
            ViewBag.Files = files.ToList();

            return View();
        }

        // GET: Files/Download
        [HttpPost]
        public ActionResult Download(string name)
        {
            var fileName = name;
            MemoryStream ms = new MemoryStream();

            try
            {
                var file = AzureFileStorage.DownloadFile(fileName);

                // Write the contents of the file to the console window.
                file.DownloadToStreamAsync(ms);
                Stream blobStream = file.OpenReadAsync().Result;
                return File(blobStream, file.Properties.ContentType, file.Name);
            }
            catch(Exception e)
            {
                ViewBag.Message = e.Message;
                return RedirectToAction("Index");
            }
        }


        // POST: Files/Delete
        [HttpPost]
        public ActionResult Delete(string name)
        {
            var fileDeleted = AzureFileStorage.ViewFiles()
                .FirstOrDefault(m => m.Name.Equals(name));

            var result = AzureFileStorage.DeleteFile(name);

            if (result)
            {
                TempData["FileDeletedSuccess"] = fileDeleted.Name;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["FileDeletedFailed"] = fileDeleted.Name;
                return new EmptyResult();
            }
        }


    }
}