using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("people")]
    public class People
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 常住总人口
        /// </summary>
        public double PermanentSum { get; set; }
        /// <summary>
        /// 城镇人口
        /// </summary>
        public double Town { get; set; }
        /// <summary>
        /// 农村人口
        /// </summary>
        public double County { get; set; }
        /// <summary>
        /// 户籍总人口
        /// </summary>
        public double HouseHold { get; set; }
        /// <summary>
        /// 户籍农业人口
        /// </summary>
        public double Agriculture { get; set; }
        /// <summary>
        /// 户籍非人口
        /// </summary>
        public double NonFram { get; set; }
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