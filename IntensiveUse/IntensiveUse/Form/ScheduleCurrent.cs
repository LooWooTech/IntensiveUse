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
    public static class ScheduleCurrent
    {
        private static int MIndex=6;

        public static IWorkbook AWrite(string filePath,ManagerCore core,int year)
        {
            var workbook = ExcelHelper.OpenWorkbook(filePath);
            var sheet = workbook.GetSheetAt(0);
            var templayeRow = sheet.GetRow(MIndex-1);
            var regions = core.RegionManager.Get();
            var dict = AMessage(core, regions, year);
            IRow row = null;
            ICell cell = null;
            int index = MIndex - 1;
            int serial = 4;
            string[] infomations = null;
            foreach(var entry in dict)
            {
                row = sheet.GetRow(index);
                if (row == null)
                {
                    row = sheet.CreateRow(index);
                }
                index++;
                infomations = entry.Key.Split('-');
                if (infomations.Count() != 8)
                {
                    continue;
                }
                for(var i = 0; i < 5; i++)
                {
                    cell = row.GetCell(i);
                    if (cell == null)
                    {
                        cell = row.CreateCell(i,templayeRow.GetCell(i).CellType);
                        cell.CellStyle = templayeRow.GetCell(i).CellStyle;
                    }
                    cell.SetCellValue(infomations[i]);
                }
                serial = 5;
                foreach(var item in entry.Value.Queues)
                {
                    cell = row.GetCell(serial);
                    if (cell == null)
                    {
                        cell = row.CreateCell(serial, templayeRow.GetCell(serial).CellType);
                        cell.CellStyle = templayeRow.GetCell(serial).CellStyle;
                    }
                    serial++;
                    cell.SetCellValue(Math.Round(item, 6));
                }
                cell = row.GetCell(serial);
                if (cell == null)
                {
                    cell = row.CreateCell(serial, templayeRow.GetCell(serial).CellType);
                    cell.CellStyle = templayeRow.GetCell(serial).CellStyle;
                }
                cell.SetCellValue(entry.Value.People.ToString());
                serial++;
                cell = row.GetCell(serial);
                if (cell == null)
                {
                    cell = row.CreateCell(serial, templayeRow.GetCell(serial).CellType);
                    cell.CellStyle = templayeRow.GetCell(serial).CellStyle;
                }
                cell.SetCellValue(entry.Value.Economy.ToString());

            }
            return workbook;
        }
        public static IWorkbook Wirte(string FilePath, ManagerCore Core,CurrentSituation Current)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow TemplateRow = sheet.GetRow(MIndex);
            var dict = Message(Core, Current);
            IRow row = null;
            ICell cell = null;
            int Index = MIndex;
            int Serial = 0;
            foreach (var key in dict.Keys)
            {
                row = sheet.GetRow(Index);
                if (row == null)
                {
                    row = sheet.CreateRow(Index);
                    //row.RowStyle = TemplateRow.RowStyle;
                }
                Serial = 0;
                cell = row.GetCell(Serial);
                if (cell == null)
                {
                    cell = row.CreateCell(Serial,TemplateRow.GetCell(Serial).CellType);
                    cell.CellStyle = TemplateRow.GetCell(Serial).CellStyle;
                }
                cell.SetCellValue(key);
                Serial++;
                foreach (var values in dict[key])
                {
                    cell = row.GetCell(Serial);
                    if (cell == null)
                    {
                        cell = row.CreateCell(Serial, TemplateRow.GetCell(Serial).CellType);
                        cell.CellStyle = TemplateRow.GetCell(Serial).CellStyle;
                    }
                    cell.SetCellValue(Math.Round(values, 6));
                    Serial++;
                }
                Index++;
            }
            return workbook;
        }

        public static Dictionary<string,Queue<Double>> Message(ManagerCore Core, CurrentSituation Current)
        {
            var dict = new Dictionary<string, Queue<double>>();
            foreach (var item in Current.Regions)
            {
                var CID = Core.ExcelManager.GetID(item);
                var exponent = Core.ExponentManager.GetTurthExponent(Current.Year, CID);
                if (!dict.ContainsKey(item))
                {
                    var queue = new Queue<double>();
                    Core.ExcelManager.Gain(exponent, ref queue);
                    dict.Add(item, queue);
                }
            }
            return dict;
        }

        public static Dictionary<string,DataSet> AMessage(ManagerCore core,List<Region> list,int year)
        {
            var dict = new Dictionary<string, DataSet>();
            foreach(var region in list)
            {
                var key = string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", region.Zone, region.Province, region.BelongCity, region.Evalutaor, region.Name,region.Code,region.Degree,region.FactorCode);
                var exponent = core.ExponentManager.GetTurthExponent(year, region.ID);//UII GCI EI API
                Situation[] situations = core.SuperiorManager.AFind(year, region);

                var landUseChange = core.EconmoyManager.AGain(year, region.ID,situations);// 
                if (!dict.ContainsKey(key))
                {
                    var queue = new Queue<double>();
                    core.ExcelManager.Gain(exponent, ref queue);
                    core.ConstructionLandManager.GetALandUseChange(landUseChange, ref queue);
                    var dataset = core.ExcelManager.GetDataSet(exponent, landUseChange, year, region);
                    dataset.Queues = queue;
                    dict.Add(key, dataset);
                }
            }
            return dict;
        }
    }
}