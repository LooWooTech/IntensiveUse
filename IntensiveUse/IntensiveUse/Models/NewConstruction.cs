using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 新增建设用地
    /// </summary>
    [Table("newconstruction")]
    public class NewConstruction
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 新增建设用地
        /// </summary>
        public double Construction { get; set; }
        /// <summary>
        /// 新增城乡建设用地
        /// </summary>
        public double Town { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
    }
}