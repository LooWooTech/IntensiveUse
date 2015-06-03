using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleBThree:ScheduleBase,ISchedule
    {
        public string Distict { get; set; }
        public ScheduleBThree()
        {
            this.Start = 2;
            this.Begin = 0;
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City, string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(Distict);
            this.City = City;
            this.Distict = Distict;
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("打开服务器模板，服务器sheet缺失");
            }

            Message(Core);
            IRow row = ExcelHelper.OpenRow(ref sheet, Start);
            ICell cell = null;
            int line=0;
            foreach (var item in DictData.Keys)
            {
                cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(item);
                var value = DictData[item];
                line=Begin+1;
                foreach (var entity in value)
                {
                    cell = row.GetCell(line++, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(Math.Round(entity, 2));
                }
            }
            row = sheet.GetRow(Start + 1);
            line=Begin+1;
            foreach (var item in Queue)
            {
                cell = row.GetCell(line++, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                
                cell.SetCellValue(Math.Round(item, 2));
            }
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
            int ID = Core.ExcelManager.GetID(this.City);
            Situation[] Cities = Core.EconmoyManager.Find(Year, ID);
            if (Cities == null || Cities.Count() != 2)
            {
                throw new ArgumentException("未读取到"+City+"相关基础数据，请告知相关人员");
            }
            double m = 0.0;
            if (Math.Abs(Cities[1].Extent - 0) > 0.001)
            {
                m = Cities[0].Extent / Cities[1].Extent;
            }
            Queue.Enqueue(Cities[0].Increment);
            Queue.Enqueue(Cities[0].Extent);
            Queue.Enqueue(Cities[1].Increment);
            Queue.Enqueue(Cities[1].Extent);
            Queue.Enqueue(m);
            var one = Core.EconmoyManager.Gain(Year, CID, Cities);
            Queue<double> mqueue = Core.ConstructionLandManager.TranslateOfLandUseChange(one);
            if (!DictData.ContainsKey(Distict))
            {
                DictData.Add(Distict, mqueue);
            }
        }
    }
}