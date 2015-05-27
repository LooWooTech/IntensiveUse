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
    }
}