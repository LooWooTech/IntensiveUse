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
        附表1A1 = 0,
        [Description("地级以上城市区域用地状况评价各区（县、市）基础数据汇总表")]
        附表1A2 = 1,
        [Description("地级以上城市人口发展与城乡建设用地变化的匹配程度分析过程表")]
        附表1A3 = 2,
        [Description("地级以上城市经济发展与建设用地变化的匹配程度分析过程表")]
        附表1A4 = 3,
        [Description("地级以上城市区域用地状况评价指标权重值表")]
        附表1A5 = 4,
        [Description("地级以上城市区域用地状况评价指标理想值汇总表")]
        附表1A6 = 5,
        [Description("地级以上城市区域用地状况整体评价指数汇总表")]
        附表1A7 = 6,
        [Description("地级以上城市所辖区（县、市）利用强度指数计算过程表")]
        附表1A8=7,
        [Description("地级以上城市所辖区（县、市）增长耗地指数计算过程表")]
        附表1A9=8,
        [Description("地级以上城市所辖区（县、市）用地弹性指数计算过程表")]
        附表1A10=9,
        [Description("地级以上城市所辖区（县、市）管理绩效指数计算过程表")]
        附表1A11=10,
        [Description("地级以上城市所辖区（县、市）评价综合指数计算汇总表")]
        附表1A12=11,
        [Description("地级以上城市所辖区（县、市）土地利用状况类型判定汇总表")]
        附表1A13=12,
        [Description("地级以上城市所辖区（县、市）土地利用状况综合类型汇总表")]
        附表1A14=13,
        [Description("县级市区域用地状况评价基础数据汇总表")]
        附表1B1=14,
        [Description("县级市人口发展与城乡建设用地变化的匹配程度分析过程表")]
        附表1B2=15,
        [Description("县级市经济发展与建设用地变化的匹配程度分析过程表")]
        附表1B3=16,
        [Description("县级市区域用地状况定量评价指标权重值表")]
        附表1B4=17,
        [Description("县级市区域用地状况定量评价指标理想值汇总表")]
        附表1B5=18,
        [Description("县级市区域用地状况整体评价指数汇总表")]
        附表1B6=19
          
    }
}