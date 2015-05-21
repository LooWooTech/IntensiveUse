using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 土地供应量
    /// </summary>
    [Table("landsupply")]
    public class LandSupply
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        public double Sum { get; set; }
        /// <summary>
        /// 新增建设用地供应面积
        /// </summary>
        public double Append { get; set; }
        /// <summary>
        /// 存量建设用地供应面积
        /// </summary>
        public double Stock { get; set; }
        /// <summary>
        /// 未利用地供应面积
        /// </summary>
        public double UnExploit { get; set; }
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