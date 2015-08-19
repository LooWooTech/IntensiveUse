using IntensiveUse.Form;
using IntensiveUse.Models;
using IntensiveUse.Helper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
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
            ViewBag.Html = Core.ExcelManager.GetDistrict("杭州市");
            return View();
        }
        public ActionResult City(string City)
        {
            ViewBag.City = City;
            ViewBag.List = Core.ExcelManager.GetDistrict(City);
            ViewBag.History = Core.StatisticsManager.Gain(City);
            return View();
        }

        public ActionResult County(string City,string County)
        {
            if (string.IsNullOrEmpty(County)||string.IsNullOrEmpty(City))
            {
                throw new ArgumentException("请选择区（县、市）之后进入,如有疑问咨询相关人员");
            }
            ViewBag.City = City;
            ViewBag.County = County;
            ViewBag.List = Core.ExcelManager.GetCity();
            ViewBag.History = Core.StatisticsManager.Gain(County);
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(UploadFileExcel Type,string Name,string City,bool Flag)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("服务器内部错误");
            }
            var FilePath = Core.FileManager.SaveFile(HttpContext,Type.ToString());
            IForm engine = null;
            switch (Type)
            {
                case UploadFileExcel.表1:
                    engine = new TableOne(Name,City);
                    break;
                case UploadFileExcel.表2:
                    engine = new TableTwo(Name);
                    break;
                case UploadFileExcel.表3:
                    engine = new TableThree(Name,City);
                    break;
                default: break;
            }
            engine.Gain(FilePath);
            if (!Core.ExcelManager.Exit(engine.GetName()))
            {
                throw new ArgumentException("未找到当前上传文件中获取到的行政区信息，或者上传当前的文件错误，请核对文件");
            }
            engine.Save(Core);
            var list = engine.GetChange();
            if (list!=null&&list.Count!=0)
            {
                ViewBag.City = City;
                ViewBag.County = Name;
                ViewBag.Flag = Flag;
                return View("Change", engine.GetChange());
            }
            if (Flag)
            {
                return RedirectToAction("City", new { City = Name });
            }
            else
            {
                return RedirectToAction("County", new { City = City, County = Name });
            }
        }

        [HttpPost]
        public ActionResult UploadOutExcel(OutputExcel Type,string City,int Year,string County)
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
                case OutputExcel.附表1B4:
                    engine = new ScheduleAFive();
                    break;
                case OutputExcel.附表1B5:
                    engine = new ScheduleASix();
                    break;
                default: break;
            }
            try
            {
                if (string.IsNullOrEmpty(County))
                {
                    engine.Read(FilePath, Core, City, Year);
                }
                else
                {
                    engine.Read(FilePath, Core, County, Year);
                }
                
            }
            catch (Exception ex)
            {
                throw new ArgumentException("在读取表格中的数据的时候，发生错误："+ex.ToString());
            }
            if (string.IsNullOrEmpty(County))
            {
                return RedirectToAction("City", new { City = City });
            }
            else
            {
                return RedirectToAction("County", new { City = City, County = County });
            }
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
            return File(fileContents, "application/ms-excel",Excel.ToString()+Excel.GetDescription()+".xls");
        }

        public ActionResult GetForDivision(string City)
        {
            List<string> html = Core.ExcelManager.GetDistrict(City);
            return HtmlResult(html);
        }

        public ActionResult DownLoadTemplet(UploadFileExcel Type)
        {
            IWorkbook workbook = null;
            MemoryStream ms = new MemoryStream();
            string filePath = Core.ExcelManager.GetExcelPath(Type.ToString()).GetAbsolutePath();
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = WorkbookFactory.Create(fs);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("打开服务器模板文件失败，失败原因："+ex.ToString());
            }
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", Type.ToString() + "模板.xls");
        }


        public ActionResult Delete(string County, string City, int Year)
        {
            int RID = 0;
            if (!string.IsNullOrEmpty(County))
            {
                RID = Core.ExcelManager.GetID(County);
            }
            else
            {
                RID = Core.ExcelManager.GetID(City);
            }
            Core.ExcelManager.Delete(Year, RID);
            if (!string.IsNullOrEmpty(County))
            {
                return RedirectToAction("County", new { City = City, County = County });
            }
            else
            {
                return RedirectToAction("City", new { City = City });
            }
            //return View();
        }

        
    }
}
