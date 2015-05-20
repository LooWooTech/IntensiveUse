using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 土地利用现状数据——建设用地
    /// </summary>
    [Table("constructionland")]
    public class ConstructionLand
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 城镇用地
        /// </summary>
        public double Town { get; set; }
        /// <summary>
        /// 采矿用地
        /// </summary>
        public double MiningLease { get; set; }
        /// <summary>
        /// 村庄 用地
        /// </summary>
        public double County { get; set; }
        /// <summary>
        /// 交通水利用地
        /// </summary>
        public double Traffic { get; set; }
        /// <summary>
        /// 其他建设用地
        /// </summary>
        public double OtherConstruction { get; set; }
        /// <summary>
        /// 其他用地
        /// </summary>
        public double Other { get; set; }
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