﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("economy")]
    public class Economy
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 地区生产总值（当年价）
        /// </summary>
        public double Current { get; set; }
        /// <summary>
        /// 地区生产总值（2010可比价）
        /// </summary>
        public double Compare { get; set; }
        /// <summary>
        /// 全社会固定资产投资总额
        /// </summary>
        public double Aggregate { get; set; }
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