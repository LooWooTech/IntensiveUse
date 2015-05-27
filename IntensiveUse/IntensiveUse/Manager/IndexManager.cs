using IntensiveUse.Helper;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class IndexManager:ManagerBase
    {
        public int[] Indexs = { 2, 5, 9, 11 };
        public int[] IndexSub = { 2, 3, 5, 6, 9, 10, 11 };
        public int Add(IndexWeight IndexWeight)
        {
            using (var db = GetIntensiveUseContext())
            {
                db.IndexWeights.Add(IndexWeight);
                db.SaveChanges();
                return IndexWeight.ID;
            }
        }


        public IndexWeight GainIndexWeight(ISheet Sheet,int Line)
        {
            double[] values = new double[4];
            int m = 0;
            foreach (var item in Indexs)
            {
                IRow row = Sheet.GetRow(item);
                if (row == null)
                {
                    continue;
                }
                ICell cell = row.GetCell(Line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    continue;
                }
                double.TryParse(ExcelHelper.GetValue(cell), out values[m++]);

            }
            return new IndexWeight()
            {
                UII = values[0],
                GCI = values[1],
                EI = values[2],
                API = values[3]
            };
        }

        public SubIndex GainSubIndex(ISheet Sheet, int Line)
        {
            double[] values = new double[IndexSub.Count()];
            int m=0;
            foreach (var item in IndexSub)
            {
                IRow row = Sheet.GetRow(item);
                if (row == null)
                {
                    continue;
                }
                ICell cell = row.GetCell(Line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    continue;
                }
                double.TryParse(ExcelHelper.GetValue(cell), out values[m++]);
            }
            Queue<double> queue = new Queue<double>();
            foreach (var item in values)
            {
                queue.Enqueue(item);
            }
            SubIndex subindex = new SubIndex();
            System.Reflection.PropertyInfo[] propList = typeof(SubIndex).GetProperties();
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    item.SetValue(subindex, queue.Dequeue(), null);
                }
            }
            return subindex; 
        }

        public Exponent GainExponent(ISheet Sheet, int Line)
        {
            return new Exponent();
        }
    }
}