using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class FileManager:ManagerBase
    {
        /// <summary>
        /// 用地保存上传的EXCEL表格
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string SaveFile(HttpContextBase context,string Name)
        {
            var file = UploadHelper.GetPostedFile(context);
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".xls" && ext != ".xlsx")
            {
                throw new ArgumentException("请上传的文件格式不对，目前平台只支持.xls以及.xlsx格式的EXCEL表格");
            }
            var filePath = UploadHelper.Upload(file);
            var fileID = Add(new UploadFile
            {
                FileName = file.FileName,
                CreateTime = DateTime.Now,
                SavePath = filePath,
                FileTypeName=Name
            });

            return UploadHelper.GetAbsolutePath(filePath); 

        }
        /// <summary>
        /// 保存文件上传记录到数据库
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        public int Add(UploadFile file)
        {
            using (var db = GetIntensiveUseContext())
            {
                db.UploadFiles.Add(file);
                db.SaveChanges();
            }
            return file.ID;
        }

        public Dictionary<int, double> Transformation<T>(List<int> Index, T exponent)
        {
            Queue<double> queue = new Queue<double>();
            Gain(exponent, ref queue);
            return Change(queue, Index);
        }

        public Dictionary<int, double> Change(Queue<double> queue, List<int> Index)
        {
            Dictionary<int, double> Dict = new Dictionary<int, double>();
            foreach (var item in Index)
            {
                if (!Dict.ContainsKey(item))
                {
                    Dict.Add(item, queue.Dequeue());
                }
            }
            return Dict;
        }

    }
}