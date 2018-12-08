using PruebaNewshore.Models.BLL.Contracts;
using PruebaNewshore.Models.BLL.Implementations;
using PruebaNewshore.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PruebaNewshore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            ViewBag.UploadStatus = "";

            if (ModelState.IsValid)
            {
                IUploadFileBLL uploadFileBLL = new UploadFileBLL();
                ActionResultDTO actionResultDTO = uploadFileBLL.ValidateUsersList(files);
                ViewBag.UploadStatus = actionResultDTO.UserMessage;
            }

            return View();
        }
    }
}