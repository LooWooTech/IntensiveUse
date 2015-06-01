using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ExponentManager:ManagerBase
    {
        public void Save(Exponent exponent)
        {
            using (var db = GetIntensiveUseContext())
            {
                Exponent entity = db.Exponents.FirstOrDefault(e => e.RID == exponent.RID && e.Year == exponent.Year&&e.Type==exponent.Type);
                if (entity == null)
                {
                    db.Exponents.Add(exponent);
                }
                else
                {
                    exponent.ID = entity.ID;
                    db.Entry(entity).CurrentValues.SetValues(exponent);
                }
                db.SaveChanges();
            }
        }

        public Exponent Create(Queue<double> queue)
        {
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

        public Exponent Find(int Year, int ID, IdealType Type)
        {
            using (var db = GetIntensiveUseContext())
            {
                return db.Exponents.FirstOrDefault(e => e.Type == Type && e.Year == Year && e.RID == ID);
                
            }
        }

        public Queue<double> Create<T>(T entity)
        {
            Queue<double> queue = new Queue<double>();
            System.Reflection.PropertyInfo[] propList = typeof(T).GetProperties();
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    double val = 0.0;
                    double.TryParse(item.GetValue(entity, null).ToString(), out val);
                    queue.Enqueue(val);
                }
            }
            return queue;
        }

        //public Queue<double> Create(Exponent exponent)
        //{
        //    Queue<double> queue = new Queue<double>();
        //    System.Reflection.PropertyInfo[] propList = typeof(Exponent).GetProperties();
        //    foreach (var item in propList)
        //    {
        //        if (item.PropertyType.Equals(typeof(double)))
        //        {
        //            double val = 0.0;
        //            double.TryParse(item.GetValue(exponent, null).ToString(), out val);
        //            queue.Enqueue(val);
        //        }
        //    }
        //    return queue;
        //}


        public Exponent GetTurthExponent(int Year, int ID)
        {
            Exponent exponent = Find(Year,ID,IdealType.Truth);
            if (exponent == null)
            {
                Queue<double> Data = Core.LandUseManager.CreateExponentQueue(Year, ID);
                exponent = Create(Data);
                exponent.Type = IdealType.Truth;
                exponent.Year = Year;
                exponent.RID = ID;
                Save(exponent);
            }

            return exponent;
        }
    }
}