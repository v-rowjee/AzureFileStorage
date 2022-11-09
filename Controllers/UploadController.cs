using FileStorage.Services;
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
            ViewBag.Directories = AzureFileStorage.GetDirectories();
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/Uploads"), _FileName);
                // save file
                file.SaveAs(_path);
                // upload file
                AzureFileStorage.UploadFile(_FileName, _path);
                // delete file
                System.IO.File.Delete(_path);

                TempData["SuccessUpload"] = _FileName;
                return View();
            }
            catch
            {
                TempData["ErrorUpload"] = "Error";
                return View();
            }
        }

    }
}