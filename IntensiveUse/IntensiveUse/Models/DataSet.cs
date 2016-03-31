using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    public class DataSet
    {
        /// <summary>
        /// 土地利用趋势类型（人口）
        /// </summary>
        public Tendency People { get; set; }
        /// <summary>
        /// 土地利用趋势类型（经济）
        /// </summary>
        public Tendency Economy { get; set; }
        public Queue<double> Queues { get; set; }
    }

    public enum Tendency
    {
        内涵挖潜型,
        集约趋势型,
        相对稳定型,
        粗放趋势型
    }
}