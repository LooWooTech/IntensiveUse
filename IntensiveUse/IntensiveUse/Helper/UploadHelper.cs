using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IntensiveUse
{
    public static class UploadHelper
    {
        private static string UploadDirectory = "Temp/";

        private static string GetAbsoluteUploadDirectory(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UploadDirectory, fileName);
        }

        public static string GetAbsolutePath(string filePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
        }
        public static HttpPostedFileBase GetPostedFile(this HttpContextBase context)
        {
            if (context.Request.Files.Count == 0)
            {
                throw new ArgumentException("请选择文件上传");
            }
            HttpPostedFileBase file = null;
            for (var i = 0; i < context.Request.Files.Count; i++)
            {
                file = context.Request.Files[i];
                if (file.ContentLength > 0)
                {
                    break;
                }
            }

            return file;
        }

        public static string Upload(this HttpPostedFileBase file)
        {
            var ext = Path.GetExtension(file.FileName);
            var fileName = file.FileName.Replace(ext, "") + "-" + DateTime.Now.Ticks.ToString() + ext;
            if (fileName.Length > 100)
            {
                fileName = fileName.Substring(fileName.Length - 100);
            }
            file.SaveAs(GetAbsoluteUploadDirectory(fileName));
            return UploadDirectory + fileName;
        }

        public static CurrentSituation GetRegions(this HttpContextBase context)
        {
            var current = new CurrentSituation();
            var Year=context.Request.Form["Year"];
            int value = 0;
            if (int.TryParse(Year, out value))
            {
                current.Year = value;
            }
            var str = context.Request.Form["Region"];
            if (!string.IsNullOrEmpty(str))
            {
                current.Regions = str.Split(',');
            }
            return current;
        }
    }
}