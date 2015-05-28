using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleBase
    {
        protected int Year { get; set; }
        protected int CID { get; set; }
        protected List<string> Disticts { get; set; }
        protected Dictionary<string, Queue<double>> DictData { get; set; }
        protected IRow TempRow { get; set; }
        protected ICell OpneCell(ref IRow row, int ID)
        {
            ICell cell = row.GetCell(ID, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = row.CreateCell(ID, TempRow.GetCell(ID).CellType);
            }
            cell.CellStyle = TempRow.GetCell(ID).CellStyle;
            return cell;
        }
    }
}