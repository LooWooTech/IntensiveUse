using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 土地利用现状数据——农用地
    /// </summary>
    [Table("agricultureland")]
    public class AgricultureLand
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 耕地
        /// </summary>
        public double Arable { get; set; }
        /// <summary>
        /// 园地
        /// </summary>
        public double Garden { get; set; }
        /// <summary>
        /// 林地
        /// </summary>
        public double Forest { get; set; }
        /// <summary>
        /// 牧草地
        /// </summary>
        public double Meadow { get; set; }
        /// <summary>
        /// 其他农用地
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