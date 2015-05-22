using IntensiveUse.Form;
using IntensiveUse.Models;
using IntensiveUse.Helper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            ViewBag.List = Core.ExcelManager.GetCity();
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
                case UploadFileExcel.表3:
                    engine = new TableThree();
                    break;
                default: break;
            }
            engine.Gain(FilePath);
            engine.Save(Core);
            return View();
        }


        [HttpPost]
        public ActionResult DownLoad(OutputExcel Excel,string City)
        {
            IWorkbook workbook=null;
            MemoryStream ms = new MemoryStream();
            workbook = Core.ExcelManager.DownLoad(Excel,City);
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", Excel.GetDescription()+".xls");
        }
    }
}
