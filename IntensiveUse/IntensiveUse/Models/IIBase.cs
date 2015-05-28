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
        public IIBase II { get; set; }
        /// <summary>
        /// 人口密度分指数值
        /// </summary>
        public double PopulationDensity { get; set; }
    }


    public class EUII
    {
        public IIBase EUII1 { get; set; }
        public IIBase EUII2 { get; set; }
        public double Economy { get; set; }
    }

    public class PEUII
    {
        public PUII PUII { get; set; }
        public EUII EUII { get; set; }
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
}