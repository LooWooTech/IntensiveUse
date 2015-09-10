using IntensiveUse.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("exponent")]
    public class Exponent
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 人口密度分指数-城乡建设用地人口密度
        /// </summary>
        public double PUII1 { get; set; }
        /// <summary>
        /// 建设用地地均固定资产投资
        /// </summary>
        public double EUII1 { get; set; }
        /// <summary>
        /// 建设用地地均地区生产总值
        /// </summary>
        public double EUII2 { get; set; }
        /// <summary>
        /// 单位人口增长消耗新增城乡建设用地量 
        /// </summary>
        public double PGCI { get; set; }
        /// <summary>
        /// 单位地区生产总值耗地下降率 
        /// </summary>
        public double EGCI1 { get; set; }
        /// <summary>
        /// 单位地区生产总值增长消耗新增建设用地量 
        /// </summary>
        public double EGCI2 { get; set; }
        /// <summary>
        /// 单位固定资产投资消耗新增建设用地量
        /// </summary>
        public double EGCI3 { get; set; }
        /// <summary>
        /// 人口与城乡建设用地增长弹性系数
        /// </summary>
        public double PEI1 { get; set; }
        /// <summary>
        /// 地区生产总值与建设用地增长弹性系数
        /// </summary>
        public double EEI { get; set; }
        /// <summary>
        /// 城市存量土地供应比率
        /// </summary>
        public double ULAPI1 { get; set; }
        /// <summary>
        /// 城市批次土地供应比率
        /// </summary>
        public double ULAPI2 { get; set; }
        /// <summary>
        /// 区分是附表1A5中的指标权重还是附表1A6中理想值
        /// </summary>
        [Column(TypeName="int")]
        public IdealType Type { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int RID { get; set; }

        public static Exponent operator /(Exponent c1, Exponent c2)
        {
            return new Exponent()
            {
                PUII1 = c1.PUII1 / c2.PUII1,
                EUII1 = c1.EUII1 / c2.EUII1,
                EUII2 = c1.EUII2 / c2.EUII2,
                PGCI = c1.PGCI / c2.PGCI,
                EGCI1 = c1.EGCI1 / c2.EGCI1,
                EGCI2 = c1.EGCI2 / c2.EGCI2,
                EGCI3 = c1.EGCI3 / c2.EGCI3,
                PEI1 = c1.PEI1 / c2.PEI1,
                EEI = c1.EEI / c2.EEI,
                ULAPI1 = c1.ULAPI1 / c2.ULAPI1,
                ULAPI2 = c1.ULAPI2 / c2.ULAPI2
            };
        }
        public static SubIndex operator *(Exponent c1, Exponent c2)
        {
            return new SubIndex()
            {
                PUII = c1.PUII1 * c2.PUII1,
                EUII = c1.EUII1 * c2.EUII1 + c1.EUII2 * c2.EUII2,
                PGCI = c1.PGCI * c2.PGCI,
                EGCI = c1.EGCI1 * c2.EGCI1 + c1.EGCI2 * c2.EGCI2 + c1.EGCI3 * c2.EGCI3,
                PEI = c1.PEI1 * c2.PEI1,
                EEI = c1.EEI * c2.EEI,
                ULAPI = c1.ULAPI1 * c2.ULAPI1 + c1.ULAPI2 * c2.ULAPI2
            };
        }

        /// <summary>
        /// 计算附表7中指标标准化值
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="Core"></param>
        /// <param name="Year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Exponent Standardized(Exponent c1, Exponent c2,ManagerCore Core,int Year,int ID)
        {
            Exponent c = c1 / c2;
            c.PGCI = 1 / c.PGCI;
            c.EGCI2 = 1 / c.EGCI2;
            c.EGCI3 = 1 / c.EGCI3;
            People people1 = Core.PeopleManager.SearchForPeople(Year, ID);//14年
            People people2 = Core.PeopleManager.SearchForPeople((Year - 1), ID);//13年
            People people3=Core.PeopleManager.SearchForPeople((Year-3),ID);//11年
            Economy economy1 = Core.EconmoyManager.SearchForEconomy(Year, ID);//14年
            Economy economy2 = Core.EconmoyManager.SearchForEconomy((Year - 1), ID);//13年
            Economy economy3 = Core.EconmoyManager.SearchForEconomy((Year - 3), ID);//11年
            ConstructionLand construction1 = Core.ConstructionLandManager.SearchForConstruction(Year, ID);//14年
            ConstructionLand construction2 = Core.ConstructionLandManager.SearchForConstruction((Year - 1), ID);//13年
            ConstructionLand construction3=Core.ConstructionLandManager.SearchForConstruction((Year-3),ID);//11年
            NewConstruction newConstruction1 = Core.NewConstructionManager.SearchForNewConstruction(Year, ID);

            System.Reflection.PropertyInfo[] propList = typeof(Exponent).GetProperties();
            double val = 0.0;
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    val = 0.0;
                    double.TryParse(item.GetValue(c, null).ToString(), out val);
                    if (val > 1)
                    {
                        item.SetValue(c, 1, null);
                    }
                    #region
                    //if (Math.Abs(val-1)>=0||Math.Abs(val-0)>=0)
                    //{
                    //    switch (item.Name)
                    //    {
                    //        case "PUII1":
                    //            c.PUII1 = 1;
                    //            break;
                    //        case "EUII1":
                    //            c.EUII1 = 1;
                    //            break;
                    //        case "EUII2":
                    //            c.EUII2 = 1;
                    //            break;
                    //        case "ULAPI1":
                    //            c.ULAPI1 = 1;
                    //            break;
                    //        case "ULAPI2":
                    //            c.ULAPI2 = 1;
                    //            break;
                    //        case "PGCI1":
                    //            if (newConstruction1.Town <= 0 && people1.PermanentSum >= people2.PermanentSum)
                    //            {
                    //                c.PGCI = 1;
                    //            }
                    //            else if (newConstruction1.Town >= 0 && people1.PermanentSum <= people2.PermanentSum)
                    //            {
                    //                c.PGCI = 0;
                    //            }
                    //            break;
                    //        case "EGCI1":
                    //            if (economy1.Compare >= economy2.Compare && construction1.SubTotal <=construction2.SubTotal)
                    //            {
                    //                c.EGCI1 = 1;
                    //            }
                    //            else if (economy1.Compare <= economy2.Compare && construction1.SubTotal >= construction2.SubTotal)
                    //            {
                    //                c.EGCI1 = 0;
                    //            }
                    //            break;
                    //        case "EGCI2":
                    //            if (newConstruction1.Construction <= 0 && economy1.Compare >= economy2.Compare)
                    //            {
                    //                c.EGCI2 = 1;
                    //            }
                    //            else if (newConstruction1.Construction >= 0 && economy1.Compare <= economy2.Compare)
                    //            {
                    //                c.EGCI2 = 0;
                    //            }
                    //            break;
                    //        case "EGCI3":
                    //            if (newConstruction1.Construction <= 0 && economy1.Aggregate >= 0)
                    //            {
                    //                c.EGCI3 = 1;
                    //            }
                    //            else if (newConstruction1.Construction >= 0 && economy1.Aggregate <= 0)
                    //            {
                    //                c.EGCI3 = 0;
                    //            }
                    //            break;
                    //        case "PEI1":
                    //            if (people1.PermanentSum >= people3.PermanentSum && construction1.Town <= construction3.Town)
                    //            {
                    //                c.PEI1 = 1;
                    //            }
                    //            else if (people1.PermanentSum <= people3.PermanentSum && construction1.Town >= construction3.Town)
                    //            {
                    //                c.PEI1 = 0;
                    //            }
                    //            break;
                    //        case "EEI":
                    //            if (economy1.Compare >= economy3.Compare && construction1.SubTotal <= construction3.SubTotal)
                    //            {
                    //                c.EEI = 1;
                    //            }
                    //            else if (economy1.Compare <= economy3.Compare && construction1.SubTotal >= construction3.SubTotal)
                    //            {
                    //                c.EEI = 0;
                    //            }
                    //            break;
                    //        default: break;
                    //    }

                    //} 
                    #endregion

                }
            }
            return c;
        }
    }

    
    public enum IdealType
    {
        [Description("指标理想值")]
        Value=0,
        [Description("指标权重")]
        Weight=2,
        [Description("指标现状值")]
        Truth=3
    }

   
}