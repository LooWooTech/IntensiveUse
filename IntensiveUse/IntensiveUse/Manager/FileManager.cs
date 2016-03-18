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
        public UploadFile SaveFile(HttpContextBase context,string Name=null)
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = "全国数据";
            }
            var file = UploadHelper.GetPostedFile(context);
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".xls" && ext != ".xlsx")
            {
                throw new ArgumentException("请上传的文件格式不对，目前平台只支持.xls以及.xlsx格式的EXCEL表格");
            }
            var filePath = UploadHelper.Upload(file);
            var uploadFile = new UploadFile
            {
                FileName = file.FileName,
                CreateTime = DateTime.Now,
                SavePath = UploadHelper.GetAbsolutePath(filePath),
                FileTypeName = Name,
                Length = GetLength(filePath),
                AnalyzeFlag = (Name == "全国数据")
            };
            var fileID = Add(uploadFile);
            return uploadFile;
            //return UploadHelper.GetAbsolutePath(filePath); 

        }

        private long GetLength(string filePath)
        {
            var absoluteFilePath = UploadHelper.GetAbsolutePath(filePath);
            var fileInfo = new FileInfo(absoluteFilePath);
            return fileInfo.Length;
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
        public int GetID(Region region)
        {
            if (region == null) return 0;
            using (var db = GetIntensiveUseContext())
            {
                var entry = db.Regions.Find(region.ID);
                if (entry == null)
                {
                    db.Regions.Add(region);
                    db.SaveChanges();
                    return region.ID;
                }
                else
                {
                    return entry.ID;
                }
                
            }
                //var entry = Core.ExcelManager.Find(region.ID);
           
        }
        public void Save<T>(List<T> list, int rid)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var t in list)
                {
                    if (t is People)
                    {
                        People m = t as People;
                        var entry = db.Peoples.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Peoples.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is Economy)
                    {
                        var m = t as Economy;
                        var entry = db.Economys.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Economys.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is AgricultureLand)
                    {
                        var m = t as AgricultureLand;
                        var entry = db.Agricultures.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);

                        if (entry == null)
                        {
                            db.Agricultures.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is ConstructionLand)
                    {
                        var m = t as ConstructionLand;
                        var entry = db.Constructions.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Constructions.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is NewConstruction)
                    {
                        var m = t as NewConstruction;
                        var entry = db.NewConstructions.FirstOrDefault(e => e.CID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.NewConstructions.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is LandSupply)
                    {
                        var m = t as LandSupply;
                        var entry = db.LandSupplys.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.LandSupplys.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is Ratify)
                    {
                        var m = t as Ratify;
                        var entry = db.Ratifys.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Ratifys.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }
                    else if (t is Superior)
                    {
                        var m = t as Superior;
                        var entry = db.Superiors.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Superiors.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }

                    db.SaveChanges();
                }
            }
        }
        public UploadFile GetAnalyzeFile()
        {
            using (var db = GetIntensiveUseContext())
            {
                return db.UploadFiles.Where(e => e.AnalyzeFlag == true).FirstOrDefault();
            }
        }

        public void SetAnalyzed(int id)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entry = db.UploadFiles.Find(id);

                if (entry != null)
                {
                    entry.AnalyzeFlag = false;
                    db.SaveChanges();
                }
            }
        }

    }
}