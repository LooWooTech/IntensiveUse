using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("statistic")]
    public class Statistic
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Year { get; set; }
        /// <summary>
        /// 人口数据
        /// </summary>
        public bool People { get; set; }
        /// <summary>
        /// 经济数据
        /// </summary>
        public bool Economy { get; set; }
        /// <summary>
        /// 农用地数据
        /// </summary>
        public bool Agriculture { get; set; }
        /// <summary>
        /// 建设用地
        /// </summary>
        public bool ConstructionLand { get; set; }
        /// <summary>
        /// 新增建设用地
        /// </summary>
        public bool NewConstruction { get; set; }
        /// <summary>
        /// 土地供应
        /// </summary>
        public bool LandSupply { get; set; }
        /// <summary>
        /// 批准批次
        /// </summary>
        public bool Ratify { get; set; }
        /// <summary>
        /// 附表6指标理想值
        /// </summary>
        public bool Ideal { get; set; }
        /// <summary>
        /// 附表5 指数权重
        /// </summary>
        public bool IndexWeight { get; set; }
        /// <summary>
        /// 附表5 分指数权重
        /// </summary>
        public bool SubIndex { get; set; }
        /// <summary>
        /// 附表5  指标权重
        /// </summary>
        public bool Weight { get; set; }
        public int RID { get; set; }
    }
}