using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    public class IIBase
    {
        /// <summary>
        /// 现状值
        /// </summary>
        public double Status { get; set; }
        /// <summary>
        /// 标准化初始值
        /// </summary>
        public double StandardInit { get; set; }
        /// <summary>
        /// 指标标准化值
        /// </summary>
        public double TargetStandard { get; set; }
        
    }

    public class PUII
    {
        /// <summary>
        /// 城乡建设用地人口密度
        /// </summary>
        public IIBase II { get; set; }
        /// <summary>
        /// 人口密度分指数值
        /// </summary>
        public double PopulationDensity { get; set; }
    }


    public class EUII
    {
        /// <summary>
        /// 建设用地地均固定资产投资
        /// </summary>
        public IIBase EUII1 { get; set; }
        /// <summary>
        /// 建设用地地均地区生产总值
        /// </summary>
        public IIBase EUII2 { get; set; }
        /// <summary>
        /// 经济强度分指数值
        /// </summary>
        public double Economy { get; set; }
    }

    public class PEUII
    {
        /// <summary>
        /// 人口密度分指数
        /// </summary>
        public PUII PUII { get; set; }
        /// <summary>
        /// 经济强度分指数
        /// </summary>
        public EUII EUII { get; set; }
        /// <summary>
        /// 利用强度指数值
        /// </summary>
        public double UII { get; set; }

        public static PEUII operator +(PEUII c1, PEUII c2)
        {
            return new PEUII()
            {
                PUII = new PUII()
                {
                    II = new IIBase()
                    {
                        Status = c1.PUII.II.Status + c2.PUII.II.Status,
                        StandardInit = c1.PUII.II.StandardInit + c2.PUII.II.StandardInit,
                        TargetStandard = c1.PUII.II.TargetStandard + c2.PUII.II.TargetStandard
                    },
                    PopulationDensity = c1.PUII.PopulationDensity + c2.PUII.PopulationDensity
                },
                EUII = new EUII()
                {
                    EUII1 = new IIBase()
                    {
                        Status = c1.EUII.EUII1.Status + c2.EUII.EUII1.Status,
                        StandardInit = c1.EUII.EUII1.StandardInit + c2.EUII.EUII1.StandardInit,
                        TargetStandard = c1.EUII.EUII1.TargetStandard + c2.EUII.EUII1.TargetStandard
                    },
                    EUII2 = new IIBase()
                    {
                        Status = c1.EUII.EUII2.Status + c2.EUII.EUII2.Status,
                        StandardInit = c1.EUII.EUII2.StandardInit + c2.EUII.EUII2.StandardInit,
                        TargetStandard = c1.EUII.EUII2.TargetStandard + c2.EUII.EUII2.TargetStandard
                    },
                    Economy = c1.EUII.Economy + c2.EUII.Economy
                },
                UII = c1.UII + c2.UII
            };
        }

        public static PEUII operator /(PEUII c1, int c2)
        {
            return new PEUII()
            {
                PUII = new PUII()
                {
                    II = new IIBase()
                    {
                        Status = c1.PUII.II.Status / c2,
                        StandardInit = c1.PUII.II.StandardInit / c2,
                        TargetStandard = c1.PUII.II.TargetStandard / c2
                    },
                    PopulationDensity = c1.PUII.PopulationDensity / c2
                },
                EUII = new EUII()
                {
                    EUII1 = new IIBase()
                    {
                        Status = c1.EUII.EUII1.Status / c2,
                        StandardInit = c1.EUII.EUII1.StandardInit / c2,
                        TargetStandard = c1.EUII.EUII1.TargetStandard / c2
                    },
                    EUII2 = new IIBase()
                    {
                        Status = c1.EUII.EUII2.Status / c2,
                        StandardInit = c1.EUII.EUII2.StandardInit / c2,
                        TargetStandard = c1.EUII.EUII2.TargetStandard / c2
                    },
                    Economy = c1.EUII.Economy / c2
                },
                UII = c1.UII / c2
            };
        }
    }

    public class PGCI
    {
        /// <summary>
        /// 单位人口增长消耗新增城乡建设用地量
        /// </summary>
        public IIBase PGCI1 { get; set; }
        /// <summary>
        /// 人口增长耗地分指数值
        /// </summary>
        public double PopulationGrowth { get; set; }
    }

    public class EGCI
    {
        /// <summary>
        /// 单位地区生产总值耗地下降率
        /// </summary>
        public IIBase EGCI1 { get; set; }
        /// <summary>
        /// 单位地区生产总值增长消耗新增建设用地量
        /// </summary>
        public IIBase EGCI2 { get; set; }
        /// <summary>
        /// 单位固定资产投资消耗新增建设用地量
        /// </summary>
        public IIBase EGCI3 { get; set; }
        /// <summary>
        /// 经济增长耗地分指数值
        /// </summary>
        public double Economy { get; set; }
    }


    public class PEGCI
    {
        /// <summary>
        /// 人口增长耗地分指数值
        /// </summary>
        public PGCI PGCI { get; set; }
        /// <summary>
        /// 经济增长耗地分指数
        /// </summary>
        public EGCI EGCI { get; set; }
        /// <summary>
        /// 增长耗地指数值
        /// </summary>
        public double GCI { get; set; }
    }
}