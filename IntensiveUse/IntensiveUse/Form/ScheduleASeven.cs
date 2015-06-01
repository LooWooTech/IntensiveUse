using IntensiveUse.Helper;
using IntensiveUse.Manager;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleASeven:ISchedule
    {
        public const int Start = 2;
        public const int Begin = 3;
        public List<int> IndexScrore { get; set; }
        public List<int> Index { get; set; }
        public int Count { get; set; }
        public Dictionary<int, Dictionary<int, double>> DictData { get; set; }
        public ScheduleASeven()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<int, Dictionary<int, double>>();
            }
            if (IndexScrore == null)
            {
                IndexScrore = new List<int>(){
                     2, 3, 5, 6, 9, 10, 11
                };
            }
            if (Index == null)
            {
                Index = new List<int>(){
                    2, 5, 9, 11
                };
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City,string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            Count = sheet.LastRowNum;
            int ID=Core.ExcelManager.GetID(City);
            Message(Core, Year, ID);
            IRow row = null;
            ICell cell = null;
            foreach (var line in DictData.Keys)
            {
                foreach (var Column in DictData[line].Keys)
                {
                    row = sheet.GetRow(Column);
                    if (row == null)
                    {
                        row = sheet.CreateRow(Column);
                    }
                    cell = row.GetCell(line,MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    if (cell == null)
                    {
                        cell = row.CreateCell(line);
                    }
                    cell.SetCellValue(Math.Round(DictData[line][Column],2));

                }
            }
            return workbook;
        }


        public void Message(ManagerCore Core,int Year,int ID)
        {
            List<int> Indexs = new List<int>();
            for (var i = Start; i <= Count; i++)
            {
                Indexs.Add(i);
            }
            Exponent[] exponent=new Exponent[4];
            exponent[0] = Core.ExponentManager.GetTurthExponent(Year,ID);
            exponent[1] = Core.ExponentManager.SearchForExponent(Year, ID, IdealType.Value);
            exponent[2] = exponent[0] / exponent[1];
            exponent[3] = exponent[0] / exponent[1];
            for (var i = 0; i < 4; i++)
            {
                DictData.Add(Begin + i, Core.FileManager.Transformation(Indexs, exponent[i]));
            }
            SubIndex subindex = Core.ExponentManager.SearchForSubIndex(Year,ID);
            SubIndex subindexVal = exponent[3] * subindex;
            DictData.Add(7, Core.FileManager.Transformation(IndexScrore, subindexVal));
            IndexWeight indexweight = Core.ExponentManager.SearchForIndexWeight(Year, ID);
            IndexWeight indexweightVal = subindexVal * indexweight;
            DictData.Add(8, Core.FileManager.Transformation(Index, indexweightVal));
            double Val = indexweightVal * indexweight;
            DictData.Add(9, new Dictionary<int, double>() { { Start, Val } });
        }


        
    }
}