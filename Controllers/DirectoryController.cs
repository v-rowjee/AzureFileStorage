using FileStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileStorage.Controllers
{

    public class DirectoryController : Controller
    {
        IAzureFileStorage AzureFileStorage;
        public DirectoryController(IAzureFileStorage AzureFileStorage)
        {
            this.AzureFileStorage = AzureFileStorage;
        }
        public DirectoryController()
        {
            this.AzureFileStorage = new AzureFileStorage();
        }


        // GET: Directory
        public ActionResult Index()
        {
            AzureFileStorage.Init();
            ViewBag.Dirs = AzureFileStorage.GetDirectories();
            return View();
        }

        // POST: Directory/CreateDir
        [HttpPost]
        public ActionResult CreateDir(string dirname)
        {
            AzureFileStorage.Init();
            var result = AzureFileStorage.CreateDirectory(dirname);

            if (result)
            {
                TempData["SuccessCreateDir"] = dirname;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorCreateDir"] = dirname;
                return RedirectToAction("Index");
            }
        }

        // POST: Directory/Delete
        [HttpPost]
        public ActionResult Delete(string dir)
        {
            AzureFileStorage.Init(dir);

            var dirDeleted = AzureFileStorage.GetDirectories()
                .FirstOrDefault(m => m.Name.Equals(dir));

            var result = AzureFileStorage.DeleteDirectory(dir);

            if (result)
            {
                TempData["SuccessDeleteDir"] = dirDeleted.Name;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorDeleteDir"] = dirDeleted.Name;
                return RedirectToAction("Index");
            }
        }

    }
}