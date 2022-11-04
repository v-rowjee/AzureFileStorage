﻿using FileStorage.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileStorage.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Uploads"), _FileName);
                // save file
                file.SaveAs(_path);
                // upload file
                AzureFileStorage.UploadFile(_FileName, _path);
                // delete file
                System.IO.File.Delete(_path);

                ViewBag.Message = "Success";
                return View();
            }
            catch
            {
                ViewBag.Message = "Error";
                return View();
            }
        }
    }
}