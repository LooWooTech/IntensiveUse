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
            ViewBag.List = Core.ExcelManager.GetCity();
            return View();
        }

        [HttpPost]
        public ActionResult City(string City)
        {
            ViewBag.City = City;
            ViewBag.List = Core.ExcelManager.GetDistrict(City);
            ViewBag.History = Core.StatisticsManager.Gain(City);
            return View();
        }

        [HttpPost]
        public ActionResult County(string County)
        {
            if (string.IsNullOrEmpty(County))
            {
                throw new ArgumentException("请选择区（县、市）之后进入");
            }
            ViewBag.County = County;
            ViewBag.List = Core.ExcelManager.GetCity();
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(UploadFileExcel Type)
        {
            var FilePath = Core.FileManager.SaveFile(HttpContext,Type.ToString());
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
            if (!Core.ExcelManager.Exit(engine.GetName()))
            {
                throw new ArgumentException("未找到当前上传文件中获取到的行政区信息，或者上传当前的文件错误，请核对文件");
            }
            engine.Save(Core);
            return View();
        }

        [HttpPost]
        public ActionResult UploadOutExcel(OutputExcel Type,string City,int Year)
        {
            var FilePath = Core.FileManager.SaveFile(HttpContext, Type.ToString());
            IRead engine = null;
            switch (Type)
            {
                case OutputExcel.附表1A5:
                    engine = new ScheduleAFive();
                    break;
                case OutputExcel.附表1A6:
                    engine = new ScheduleASix();
                    break;
                default: break;
            }
            try
            {
                engine.Read(FilePath, Core, City, Year);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("在读取表格中的数据的时候，发生错误："+ex.ToString());
            }

            return View("UploadFile");
        }


        [HttpPost]
        public ActionResult DownLoad(OutputExcel Excel,int Year,string City,string County)
        {
            IWorkbook workbook=null;
            MemoryStream ms = new MemoryStream();
            workbook = Core.ExcelManager.DownLoad(Excel,Year,City,County);
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", Excel.GetDescription()+".xls");
        }

        public ActionResult GetForDivision(string City)
        {
            List<string> html = Core.ExcelManager.GetDistrict(City);
            return HtmlResult(html);
        }

        
    }
}
