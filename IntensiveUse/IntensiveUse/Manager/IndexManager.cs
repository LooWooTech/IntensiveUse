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

        public void Save(IndexWeight indexWeight)
        {
            using (var db = GetIntensiveUseContext())
            {
                IndexWeight entity = db.IndexWeights.FirstOrDefault(e => e.RID == indexWeight.RID && e.Year == indexWeight.Year);
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
                SubIndex entity = db.SubIndexs.FirstOrDefault(e => e.RID == SubIndex.RID && e.Year == SubIndex.Year);
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

        public void WriteIndexWeight(ref ISheet Sheet, int line,int Year,int ID)
        {
            IndexWeight indexweight = SearchForIndexWeight(Year, ID);
            Queue<double> queue = new Queue<double>();
            Gain(indexweight, ref queue);
            foreach (var item in Indexs)
            {
                IRow row = Sheet.GetRow(item);
                if (row == null)
                {
                    throw new ArgumentException("在保存指数权重数据到表格附表1A5或者1B4的时候，未读取到相关的行，请告知相关人员");
                }
                ICell cell = row.GetCell(line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    throw new ArgumentException("在保存指数权重数据到表格附表1A5或者1B4的时候，未读取到相关的列，请告知相关人员");
                }
                cell.SetCellValue(Math.Round(queue.Dequeue(), 2));
            }
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

        public void WriteSubIndex(ref ISheet Sheet, int Line, int Year, int ID)
        {
            SubIndex subindex = SearchForSubIndex(Year, ID);
            Queue<double> queue = new Queue<double>();
            Gain(subindex, ref queue);
            foreach (var item in IndexSub)
            {
                IRow row = Sheet.GetRow(item);
                if (row == null)
                {
                    throw new ArgumentException("在保存分指数数据到表格附表1A5或者1B4的时候，未读取到相关的行，请告知相关人员");
                }
                ICell cell = row.GetCell(Line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    throw new ArgumentException("在保存分指数数据到表格附表1A5或者1B4的时候，未读取到相关的列，请告知相关人员");
                }
                cell.SetCellValue(Math.Round(queue.Dequeue(), 2));
            }
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


        public Foundation GainFoundation(ISheet Sheet, int Line,int Begin)
        {
            Queue<string> queue = new Queue<string>();
            for (var i = 0; i < 11; i++)
            {
                IRow row = Sheet.GetRow(Begin + i);
                if (row == null)
                {
                    throw new ArgumentException("在读取理想值依据列的时候，未获取到相关的行，请告知相关人员");
                }
                ICell cell = row.GetCell(Line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    throw new ArgumentException("在读取理想值依据列的时候，未获取到相关的列，请告知相关人员");
                }
                queue.Enqueue(cell.ToString());
            }
            Foundation foundation = new Foundation();
            System.Reflection.PropertyInfo[] propList = typeof(Foundation).GetProperties();
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(string)))
                {
                    item.SetValue(foundation, queue.Dequeue(), null);
                }
            }
            return foundation;
        }

        public void WriteExponent(ref ISheet Sheet, int Line, int Begin, int Year, int ID)
        {
            Exponent exponent = SearchForExponent(Year, ID, IdealType.Weight);
            Queue<double> queue = new Queue<double>();
            Gain(exponent, ref queue);
            int Count = queue.Count;
            for (var i = 0; i < Count; i++)
            {
                IRow row = Sheet.GetRow(Begin + i);
                if (row == null)
                {
                    throw new ArgumentException("在保存指标权重数据到附表1A5或者附表1B4的时候，未读取到相关的行，请告知相关人员");
                }
                ICell cell = row.GetCell(Line, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    throw new ArgumentException("在保存指标权重数据到附表1A5或者附表1B4的时候，未读取到相关的列，请告知相关人员");
                }
                cell.SetCellValue(Math.Round(queue.Dequeue(), 2));
            }
        }

        public void Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.IndexWeights.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.IndexWeights.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

    }
}