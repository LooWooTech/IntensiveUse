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
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City,string Distict,int[] Indexs)
        {
            if (Indexs == null || Indexs.Count() != 7)
            {
                throw new ArgumentException("精度位数据为null，或者为空，无法进行数据填写！");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            Count = sheet.LastRowNum;
            int ID = 0;
            if (string.IsNullOrEmpty(Distict))
            {
                ID = Core.ExcelManager.GetID(City);
            }
            else
            {
                ID = Core.ExcelManager.GetID(Distict);
            }
            
            Message(Core, Year, ID);
            IRow row = null;
            ICell cell = null;
            int Serial = 0;
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
                    cell.SetCellValue(Math.Round(DictData[line][Column],Indexs[Serial]));

                }
                Serial++;
            }
            return workbook;
        }

        public IWorkbook AWrite(string filePath, ManagerCore core, int year, string province, string belongCity, string name, int[] indexs)
        {
            return null;
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
            exponent[3] = Exponent.Standardized(exponent[0], exponent[1], Core, Year, ID);
            for (var i = 0; i < 4; i++)
            {
                DictData.Add(Begin + i, Core.FileManager.Transformation(Indexs, exponent[i]));
            }
            var exponWeight = Core.ExponentManager.SearchForExponent(Year, ID, IdealType.Weight);
            //分指数值
            SubIndex subindexVal = exponent[3] * exponWeight * 100;
            DictData.Add(7, Core.FileManager.Transformation(IndexScrore, subindexVal));
            SubIndex subindex = Core.ExponentManager.SearchForSubIndex(Year,ID);
            //指数值
            IndexWeight indexweightVal = subindexVal * subindex;
            DictData.Add(8, Core.FileManager.Transformation(Index, indexweightVal));
            IndexWeight indexweight = Core.ExponentManager.SearchForIndexWeight(Year, ID);
            //总指数
            double Val = indexweightVal * indexweight;
            DictData.Add(9, new Dictionary<int, double>() { { Start, Val } });
        }


        
    }
}