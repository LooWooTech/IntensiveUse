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
        /// 小计  建设用地面积
        /// </summary>
        public double SubTotal { get; set; }
        /// <summary>
        /// 城乡建设用地面积  （全国数据导入增加）
        /// </summary>
        public double TowCouConstruction { get; set; }
        /// <summary>
        /// 城镇工矿用地面积  （全国数据导入增加）
        /// </summary>
        public double TownMiningLease { get; set; }
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
        /// 总面积
        /// </summary>
        public double Sum { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int RID { get; set; }

    }
}