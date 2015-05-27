using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("indexweight")]
    public class IndexWeight
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 利用强度指数
        /// </summary>
        public double UII { get; set; }
        /// <summary>
        /// 增长耗地指数
        /// </summary>
        public double GCI { get; set; }
        /// <summary>
        /// 用地弹性指数
        /// </summary>
        public double EI { get; set; }
        /// <summary>
        /// 管理绩效指数
        /// </summary>
        public double API { get; set; }
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