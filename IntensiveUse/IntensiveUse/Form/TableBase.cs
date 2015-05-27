using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IntensiveUse.Form
{
    public class TableBase
    {
        public string Name { get; set; }
        

        private  Regex _yearRe = new Regex(@"^20[0-9]{2}", RegexOptions.Compiled);
        public  bool VerificationYear(string value)
        {
            return _yearRe.IsMatch(value);
        }

        

        public IRow RowGet(ISheet sheet,int ID)
        {
            IRow row = sheet.GetRow(ID);
            if (row == null)
            {
                throw new ArgumentException("未获取相关EXCEL表格行，请核对表格数据，如有冲突请联系相关人员");
            }
            return row;
        }
    }
}