using IntensiveUse.Form;
using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntensiveUse.Controllers
{
    public class HomeController : ControllerBase
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult City()
        {
            return View();
        }

        public ActionResult County()
        {
            return View();
        }


        public ActionResult UploadFile(UploadFileExcel Type)
        {
            var FilePath = Core.FileManager.SaveFile(HttpContext);
            IForm engine = null;
            switch (Type)
            {
                case UploadFileExcel.表1:
                    engine = new TableOne();
                    break;
                case UploadFileExcel.表2:
                    engine = new TableTwo();
                    break;
                default: break;
            }
            engine.Gain(FilePath);
            engine.Save(Core);
            return View();
        }
    }
}
