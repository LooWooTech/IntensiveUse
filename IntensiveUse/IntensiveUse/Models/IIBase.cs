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

        public static PEGCI operator +(PEGCI c1, PEGCI c2)
        {
            return new PEGCI()
            {
                PGCI = new PGCI()
                {
                    PGCI1 = new IIBase()
                    {
                        Status = c1.PGCI.PGCI1.Status + c2.PGCI.PGCI1.Status,
                        StandardInit = c1.PGCI.PGCI1.StandardInit + c2.PGCI.PGCI1.StandardInit,
                        TargetStandard = c1.PGCI.PGCI1.TargetStandard + c2.PGCI.PGCI1.TargetStandard
                    },
                    PopulationGrowth = c1.PGCI.PopulationGrowth + c2.PGCI.PopulationGrowth
                },
                EGCI = new EGCI()
                {
                    EGCI1 = new IIBase()
                    {
                        Status = c1.EGCI.EGCI1.Status + c2.EGCI.EGCI1.Status,
                        StandardInit = c1.EGCI.EGCI1.StandardInit + c2.EGCI.EGCI1.StandardInit,
                        TargetStandard = c1.EGCI.EGCI1.TargetStandard + c2.EGCI.EGCI1.TargetStandard
                    },
                    EGCI2 = new IIBase()
                    {
                        Status = c1.EGCI.EGCI2.Status + c2.EGCI.EGCI2.Status,
                        StandardInit = c1.EGCI.EGCI2.StandardInit + c2.EGCI.EGCI2.StandardInit,
                        TargetStandard = c1.EGCI.EGCI2.TargetStandard + c2.EGCI.EGCI2.TargetStandard
                    },
                    EGCI3 = new IIBase()
                    {
                        Status = c1.EGCI.EGCI3.Status + c2.EGCI.EGCI3.Status,
                        StandardInit = c1.EGCI.EGCI3.StandardInit + c2.EGCI.EGCI3.StandardInit,
                        TargetStandard = c1.EGCI.EGCI3.TargetStandard + c2.EGCI.EGCI3.TargetStandard
                    },
                    Economy = c1.EGCI.Economy + c2.EGCI.Economy
                },
                GCI = c1.GCI + c2.GCI
            };
        }

        public static PEGCI operator /(PEGCI c1, int c2)
        {
            return new PEGCI()
            {
                PGCI = new PGCI()
                {
                    PGCI1 = new IIBase()
                    {
                        Status = c1.PGCI.PGCI1.Status / c2,
                        StandardInit = c1.PGCI.PGCI1.StandardInit / c2,
                        TargetStandard = c1.PGCI.PGCI1.TargetStandard / c2
                    },
                    PopulationGrowth = c1.PGCI.PopulationGrowth / c2
                },
                EGCI = new EGCI()
                {
                    EGCI1 = new IIBase()
                    {
                        Status = c1.EGCI.EGCI1.Status / c2,
                        StandardInit = c1.EGCI.EGCI1.StandardInit / c2,
                        TargetStandard = c1.EGCI.EGCI1.TargetStandard / c2
                    },
                    EGCI2 = new IIBase()
                    {
                        Status = c1.EGCI.EGCI2.Status / c2,
                        StandardInit = c1.EGCI.EGCI2.StandardInit / c2,
                        TargetStandard = c1.EGCI.EGCI2.TargetStandard / c2
                    },
                    EGCI3 = new IIBase()
                    {
                        Status = c1.EGCI.EGCI3.Status / c2,
                        StandardInit = c1.EGCI.EGCI3.StandardInit / c2,
                        TargetStandard = c1.EGCI.EGCI3.TargetStandard / c2
                    },
                    Economy = c1.EGCI.Economy / c2
                },
                GCI = c1.GCI / c2
            };
        }
    }

    public class PEI
    {
        /// <summary>
        /// 人口与城乡建设用地增长弹性系数
        /// </summary>
        public IIBase PEI1 { get; set; }
        /// <summary>
        /// 人口用地弹性分指数值
        /// </summary>
        public double PopulationSite { get; set; }
    }

    public class EEI
    {
        /// <summary>
        /// 地区生产总值与建设用地增长弹性系数
        /// </summary>
        public IIBase EEI1 { get; set; }
        /// <summary>
        /// 经济用地弹性分指数值
        /// </summary>
        public double Economy { get; set; }
    }

    public class PEEI
    {
        /// <summary>
        /// 人口用地弹性分指数
        /// </summary>
        public PEI PEI { get; set; }
        /// <summary>
        /// 经济用地弹性分指数
        /// </summary>
        public EEI EEI { get; set; }
        /// <summary>
        /// 用地弹性指数值
        /// </summary>
        public double EI { get; set; }

        public static PEEI operator +(PEEI c1, PEEI c2)
        {
            return new PEEI()
            {
                PEI = new PEI()
                {
                    PEI1 = new IIBase()
                    {
                        Status = c1.PEI.PEI1.Status + c2.PEI.PEI1.Status,
                        StandardInit = c1.PEI.PEI1.StandardInit + c2.PEI.PEI1.StandardInit,
                        TargetStandard = c1.PEI.PEI1.TargetStandard + c2.PEI.PEI1.TargetStandard
                    },
                    PopulationSite = c1.PEI.PopulationSite + c2.PEI.PopulationSite
                },
                EEI = new EEI()
                {
                    EEI1 = new IIBase()
                    {
                        Status = c1.EEI.EEI1.Status + c2.EEI.EEI1.Status,
                        StandardInit = c1.EEI.EEI1.StandardInit + c2.EEI.EEI1.StandardInit,
                        TargetStandard = c1.EEI.EEI1.TargetStandard + c2.EEI.EEI1.TargetStandard
                    },
                    Economy = c1.EEI.Economy + c2.EEI.Economy
                },
                EI = c1.EI + c2.EI
            };
        }

        public static PEEI operator /(PEEI c1, int c2)
        {
            return new PEEI()
            {
                PEI = new PEI()
                {
                    PEI1 = new IIBase()
                    {
                        Status = c1.PEI.PEI1.Status / c2,
                        StandardInit = c1.PEI.PEI1.StandardInit / c2,
                        TargetStandard = c1.PEI.PEI1.TargetStandard / c2
                    },
                    PopulationSite = c1.PEI.PopulationSite / c2
                },
                EEI = new EEI()
                {
                    EEI1 = new IIBase()
                    {
                        Status = c1.EEI.EEI1.Status / c2,
                        StandardInit = c1.EEI.EEI1.StandardInit / c2,
                        TargetStandard = c1.EEI.EEI1.TargetStandard / c2
                    },
                    Economy = c1.EEI.Economy / c2
                },
                EI = c1.EI / c2
            };
        }
    }
}