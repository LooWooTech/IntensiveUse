﻿using IntensiveUse.Helper;
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

        public void Save(IndexWeight indexWeight)
        {
            using (var db = GetIntensiveUseContext())
            {
                IndexWeight entity = db.IndexWeights.FirstOrDefault(e => e.RID == indexWeight.RID && e.Year.ToLower() == indexWeight.Year.ToLower());
                if (entity == null)
                {
                    db.IndexWeights.Add(indexWeight);
                }
                else
                {
                    indexWeight.ID = entity.ID;
                    db.Entry(entity).CurrentValues.SetValues(indexWeight);
                }
                db.SaveChanges();
            }
        }


        public void Save(SubIndex SubIndex)
        {
            using (var db = GetIntensiveUseContext())
            {
                SubIndex entity = db.SubIndexs.FirstOrDefault(e => e.RID == SubIndex.RID && e.Year.ToLower() == SubIndex.Year.ToLower());
                if (entity == null)
                {
                    db.SubIndexs.Add(SubIndex);
                }
                else
                {
                    SubIndex.ID = entity.ID;
                    db.Entry(entity).CurrentValues.SetValues(SubIndex);
                }
                db.SaveChanges();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sheet"></param>
        /// <param name="Line">获取数据所在的列</param>
        /// <param name="Begin">获取数据开始的行</param>
        /// <returns></returns>
        public  Exponent GainExponent(ISheet Sheet, int Line,int Begin)
        {
            Queue<double> queue = new Queue<double>();
            IRow row = null;
            ICell cell = null;
            string value = string.Empty;
            int Count = Sheet.LastRowNum;
            for (var i = Begin; i <= Count; i++)
            {
                row = Sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }
                cell = row.GetCell(Line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double m = 0.0;
                value = cell.ToString().Replace("%", "").Trim();
                double.TryParse(value, out m);
                queue.Enqueue(m);
            }
            Exponent exponent = new Exponent();
            System.Reflection.PropertyInfo[] propList = typeof(Exponent).GetProperties();
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    item.SetValue(exponent, queue.Dequeue(), null);
                }
            }
            return exponent;
        }

    }
}