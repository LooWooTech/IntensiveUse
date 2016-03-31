using IntensiveUse.Helper;
using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class EconomyManager:ManagerBase
    {
        public double[] Get(int Year, int ID)
        {
            Economy economy = SearchForEconomy(Year, ID);
            return new double[] { economy.Current, economy.Compare, economy.Aggregate };
        }

        public LandUseChange Gain(int Year,int ID,Situation[] Citys)
        {
            if (Citys.Count() != 2)
            {
                throw new ArgumentException("未读取到上一级行政区相关数据");
            }
            Situation[] values = Find(Year,ID);
            if (values == null||values.Count()!=2)
            {
                throw new ArgumentException("读取数据失败");
            }
            double m = 0.0,n=0.0;
            if (Math.Abs(values[1].Extent - 0) > 0.001)
            {
                m = values[0].Extent / values[1].Extent;
            }
            if (Math.Abs(Citys[0].Increment - 0) > 0.001 && Math.Abs(Citys[1].Increment - 0) > 0.001&&Math.Abs(values[0].Increment-0)>0.001&&Math.Abs(values[1].Increment-0)>0.001)
            {
                n = values[0].Increment * Citys[1].Increment / Citys[0].Increment / values[1].Increment;
            }
            return new LandUseChange()
            {
                ESituation = values[0],
                CSituation = values[1],
                EEI1 = m,
                ECI1 = n
            };
        }
        public LandUseChange AGain(int year,int rid,Situation[] situations)
        {
            Situation[] values = Find(year, rid);
            if (values == null || values.Count() != 2)
            {
                throw new ArgumentException("读取数据失败！");
            }
            double m = 0.0, n = 0.0;
            if (Math.Abs(values[1].Extent - 0) > 0.001)
            {
                m = values[0].Extent / values[1].Extent;
            }
            if (Math.Abs(situations[1].Increment - 0) > 0.001 && Math.Abs(situations[0].Increment - 0) > 0.001 && Math.Abs(values[0].Increment - 0) > 0.001 && Math.Abs(values[1].Increment - 0) > 0.001)
            {
                n = values[0].Increment * situations[1].Increment / situations[0].Increment / values[1].Increment;
            }
            
            return new LandUseChange
            {
                ESituation = values[0],
                CSituation = values[1],
                EEI1 = m,
                ECI1=n
            };
        }

        public Situation[] Find(int Year, int ID)
        {
            int em = Year;
            Economy economy1 = SearchForEconomy(em, ID);
            ConstructionLand construction1 = SearchForConstruction(em, ID);
            em = (Year - 3);
            Economy economy2 = SearchForEconomy(em, ID);
            ConstructionLand construction2 = SearchForConstruction(em, ID);
            Situation[] situation = new Situation[2];
            situation[0] = new Situation()
            {
                Increment = economy1.Compare - economy2.Compare//year 年与year-3 年的可比价差
            };
            situation[1] = new Situation()
            {
                Increment = construction1.SubTotal - construction2.SubTotal// year年与year-3 年的 建设用地纵面
            };
            if (Math.Abs(economy2.Compare - 0) > 0.001)
            {
                situation[0].Extent = situation[0].Increment / economy2.Compare * 100;
            }
            if (Math.Abs(construction2.SubTotal - 0) > 0.001)
            {
                situation[1].Extent = situation[1].Increment / construction2.SubTotal * 100;
            }
            return situation;
        }


        public double[] GetEUII(int Year, int ID)
        {
            double sum = 0.0;
            int em = 0 ;
            Economy economy = null;
            for (var i = 2; i >= 0; i--)
            {
                em = Year - i;
                economy = SearchForEconomy(em, ID);
                sum += economy.Aggregate;
            }
            sum = sum / 3;
            em=Year;
            ConstructionLand construction = SearchForConstruction(em,ID);
            economy = SearchForEconomy(em, ID);
            double[] values = new double[2];
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                values[0] = sum * 100 / construction.SubTotal;
                values[1] = economy.Current * 100 / construction.SubTotal;
            }
            return values;
        }

        public double[] GetEGCI(int Year, int ID)
        {
            double[] values = new double[3];
            Economy economy1 = SearchForEconomy((Year - 1), ID);//13年
            Economy economy2 = SearchForEconomy(Year, ID);//14年
            ConstructionLand construction1=SearchForConstruction((Year-1),ID);
            ConstructionLand construction2=SearchForConstruction(Year,ID);
            if (Math.Abs(economy1.Compare - 0) > 0.001 && Math.Abs(economy2.Compare - 0) > 0.001&&Math.Abs(construction1.SubTotal-0)>0.001)
            {
                values[0] = (construction1.SubTotal / economy1.Compare - construction2.SubTotal / economy2.Compare) * economy1.Compare / construction1.SubTotal * 100;
            }
            NewConstruction newconstruction = SearchForNewConstruction(Year, ID);
            if (Math.Abs(economy2.Compare - economy1.Compare) > 0.001)
            {
                values[1] = newconstruction.Construction * 10000 / (economy2.Compare - economy1.Compare);
            }
            if (Math.Abs(economy2.Aggregate - 0) > 0.001)
            {
                values[2] = newconstruction.Construction * 10000 / economy2.Aggregate;
            }
            return values;
        }


        public Situation[] EEI(int Year, int ID)
        {
            Situation[] situation=new Situation[2];
            Economy economy1 = SearchForEconomy(Year, ID);
            Economy economy2 = SearchForEconomy((Year - 3), ID);
            ConstructionLand construction1 = SearchForConstruction(Year,ID);
            ConstructionLand construction2 = SearchForConstruction((Year - 3), ID);
            situation[0] = new Situation()
            {
                Increment = economy1.Compare - economy2.Compare
            };
            situation[1] = new Situation()
            {
                Increment = construction1.SubTotal - construction2.SubTotal
            };
            if (Math.Abs(economy2.Compare - 0) > 0.001)
            {
                situation[0].Extent = situation[0].Increment / economy2.Compare*100;
            }
            if (Math.Abs(construction2.SubTotal - 0) > 0.001)
            {
                situation[1].Extent = situation[1].Increment / construction2.SubTotal * 100;
            }
            return situation;
        }

        public bool Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.Economys.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.Economys.Remove(entity);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }

        public List<EconomyChange> Superior(Dictionary<int, Economy> Dict)
        {
            List<EconomyChange> list = new List<EconomyChange>();
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in Dict.Values)
                {
                    var entity = db.Economys.FirstOrDefault(e => e.RID == item.RID && e.Year == item.Year);
                    if (entity != null)
                    {
                        if (entity.Compare > 0&&Math.Abs(entity.Compare-item.Compare)>=0.01)
                        {
                            list.Add(new EconomyChange()
                            {
                                Original = entity.Compare,
                                BrandNew = item.Compare,
                                Year = item.Year,
                                Type = DataType.Compare
                            });
                           
                        }

                        if (entity.Current > 0 && Math.Abs(entity.Current - item.Current) >= 0.01)
                        {
                            list.Add(new EconomyChange()
                            {
                                Original = entity.Current,
                                BrandNew = item.Current,
                                Year = item.Year,
                                Type = DataType.Current
                            });
                        }
                        entity.Current = item.Current;
                        entity.Compare = item.Compare;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Economys.Add(item);
                        db.SaveChanges();
                    }

                }
            }

            return list;
        }
        
    }
}