using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("outputlist")]
    public class OutPutList
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OutputExcel Type { get; set; }

    }
      public enum OutputExcel
    {
        [Description("地级以上城市区域用地状况评价基础数据汇总表")]
        附表1A1=0,
        [Description("地级以上城市区域用地状况各（县、市）基础数据汇总表")]
        附表1A2=1,
        [Description("地级以上城市人口发展与城乡建设用地变化的匹配程度分析过程表")]
        附表1A3 = 2,
        [Description("地级以上城市经济发展与建设用地变化的匹配程度分析过程表")]
        附表1A4 = 3,
        [Description("地级以上城市（县级市）区域用地状况定量")]
        附表1A5=4,
        [Description("地级以上城市（县级市）区域用地状况定量评价指标理想值汇总表")]
        附表1A6=5,
        [Description("地级以上城市（县级市）区域用地状况整体评价指数汇总表")]
        附表1A7=6,
        [Description("县级市人口发展与城乡建设用地变化的匹配程度分析过程表")]
        附表1B2=7,
        [Description("县级市经济发展与建设用地变化的匹配程度分析过程表")]
        附表1B3=8
    }
}