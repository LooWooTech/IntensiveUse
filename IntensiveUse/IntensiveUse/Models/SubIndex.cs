using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("subindex")]
    public class SubIndex
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 人口密度分指数
        /// </summary>
        public double PUII { get; set; }
        /// <summary>
        /// 经济强度分指数
        /// </summary>
        public double EUII { get; set; }
        /// <summary>
        /// 人口增长耗地指数
        /// </summary>
        public double PGCI { get; set; }
        /// <summary>
        /// 经济增长耗地分指数
        /// </summary>
        public double EGCI { get; set; }
        /// <summary>
        /// 人口用地弹性分指数
        /// </summary>
        public double PEI { get; set; }
        /// <summary>
        /// 经济用地弹性分指数
        /// </summary>
        public double EEI { get; set; }
        /// <summary>
        /// 城市用地管理绩效分指数
        /// </summary>
        public double ULAPI { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int RID { get; set; }
    }
}