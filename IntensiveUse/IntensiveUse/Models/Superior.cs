using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("superiors")]
    public class Superior
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 所属省地区生产总值 当年价
        /// </summary>
        public double ProvinceCurrent { get; set; }
        /// <summary>
        /// 所属省地区生产总值（2010年可比价）
        /// </summary>
        public double ProvinceCompare { get; set; }
        /// <summary>
        /// 所属省建设用地总面积
        /// </summary>
        public double ProvinceConstruction { get; set; }
        /// <summary>
        /// 所属地级市建设用地总面积
        /// </summary>
        public double CityConstruction { get; set; }
        /// <summary>
        /// 所属地级市地区生产总值（当年价）
        /// </summary>
        public double CityCurrent { get; set; }
        /// <summary>
        /// 所属地级市地区生产总值（2010可比价）
        /// </summary>
        public double CityCompare { get; set; }
        public int Year { get; set; }
        public int RID { get; set; }

    }
}