using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleBase
    {
        protected int Start { get; set; }
        protected int Begin { get; set; }
        protected int Year { get; set; }
        protected int CID { get; set; }
        protected string City { get; set; }
        protected List<string> Disticts { get; set; }
        protected List<int> Columns { get; set; }
        protected Dictionary<string, Queue<double>> DictData { get; set; }
        

        protected Queue<double> Queue { get; set; }
        protected IRow TempRow { get; set; }

        public ScheduleBase()
        {
            
            if (DictData == null)
            {
                DictData = new Dictionary<string, Queue<double>>();
            }
            if (Columns == null)
            {
                Columns = new List<int>();
            }
            if (Disticts == null)
            {
                Disticts = new List<string>();
            }
            if (Queue == null)
            {
                Queue = new Queue<double>();
            }
        }

        protected IRow OpenRow(ref ISheet Sheet, int ID)
        {
            IRow row = Sheet.GetRow(ID);
            if (row == null)
            {
                row = Sheet.CreateRow(ID);
                if (TempRow.RowStyle != null)
                {
                    row.RowStyle = TempRow.RowStyle;
                }

            }
            return row;
        }
        protected ICell OpenCell(ref IRow row, int ID)
        {
            ICell cell = row.GetCell(ID, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = row.CreateCell(ID, TempRow.GetCell(ID).CellType);
            }
            cell.CellStyle = TempRow.GetCell(ID).CellStyle;
            return cell;
        }

        protected void WriteBase(ref ISheet sheet,int Number)
        {
            IRow row = sheet.GetRow(Number);
            ICell cell = null;
            foreach (var m in Columns)
            {
                cell = row.GetCell(m, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(Queue.Dequeue(), 2));
            }
            sheet.ShiftRows(Number+1, sheet.LastRowNum, DictData.Count - 1);
            int Serial = 0;
            foreach (var item in DictData.Keys)
            {
                row = OpenRow(ref sheet, Serial + Number);
                cell = OpenCell(ref row, 0);
                cell.SetCellValue(++Serial);
                cell = OpenCell(ref row, 1);
                cell.SetCellValue(item);
                foreach (var m in Columns)
                {
                    cell = OpenCell(ref row, m);
                    cell.SetCellValue(Math.Round(DictData[item].Dequeue(), 2));
                }
            }
        }
    }
}