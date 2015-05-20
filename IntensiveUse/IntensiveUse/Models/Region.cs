using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("region")]
    public class Region
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 行政区名
        /// </summary>
        [MaxLength(1023)]
        public string Name { get; set; }
        /// <summary>
        /// 行政区代码
        /// </summary>
        [MaxLength(255)]
        public string Code { get; set; }
    }
}