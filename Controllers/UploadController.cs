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
        IAzureFileStorage AzureFileStorage;
        public UploadController(IAzureFileStorage AzureFileStorage)
        {
            this.AzureFileStorage = AzureFileStorage;
        }
        public UploadController()
        {
            this.AzureFileStorage = new AzureFileStorage();
        }


        // GET: Upload
        public ActionResult Index()
        {
            AzureFileStorage.Init();
            ViewBag.Directories = AzureFileStorage.GetDirectories();
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(string dir, HttpPostedFileBase file)
        {

            try
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/Uploads"), _FileName);
                // save file
                file.SaveAs(_path);
                // upload file
                AzureFileStorage.Init(dir);
                AzureFileStorage.UploadFile(_FileName, _path);
                // delete file
                System.IO.File.Delete(_path);

                TempData["SuccessUpload"] = _FileName;
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["ErrorUpload"] = "Error";
                return RedirectToAction("Index");
            }
        }

        // POST: Upload/CreateDir
        [HttpPost]
        public JsonResult CreateDir(string dirname)
        {
            AzureFileStorage.Init();
            var result = AzureFileStorage.CreateDirectory(dirname);

            return Json(new { result = result });
        }

    }
}