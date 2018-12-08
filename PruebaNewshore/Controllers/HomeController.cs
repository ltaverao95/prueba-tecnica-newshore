using PruebaNewshore.Models;
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
                ActionResultReturnDTO<FileInfo> actionResultReturnDTO = uploadFileBLL.ValidateUsersList(files);
                if (actionResultReturnDTO.HasErrors)
                {
                    ViewBag.UploadStatus = actionResultReturnDTO.UserMessage;
                    return View();
                }

                ViewBag.UploadStatus = "Procesamiento exitoso";

                byte[] fileData = null;

                using (BinaryReader binaryReader = new BinaryReader(actionResultReturnDTO.ResultData.OpenRead()))
                {
                    fileData = binaryReader.ReadBytes((int)actionResultReturnDTO.ResultData.Length);
                }

                return File(fileData, "text/plain", Constants.ResultsFileName);
            }

            return View();
        }
    }
}