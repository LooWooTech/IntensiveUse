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
            

            //ScheduleASix engine = new ScheduleASix();
            //Exponent exponent=engine.Read(FilePath);
            //if (exponent == null)
            //{
            //    throw new ArgumentException("未读取到相关理想数据");
            //}
            //exponent.Year = Year.ToString();
            //exponent.RID = Core.ExcelManager.GetID(City);
            //int ID = Core.ExponentManager.Add(exponent);
            //if (ID <= 0)
            //{
            //    throw new ArgumentException("保存理想值失败");
            //}
            return View("UploadFile");
        }


        [HttpPost]
        public ActionResult DownLoad(OutputExcel Excel,int Year,string City,string County)
        {
            IWorkbook workbook=null;
            MemoryStream ms = new MemoryStream();
            workbook = Core.ExcelManager.DownLoad(Excel,Year,City);
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
