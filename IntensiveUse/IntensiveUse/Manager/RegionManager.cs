﻿using IntensiveUse.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class RegionManager:ManagerBase
    {
        private string ConnectionString { get; set; }
        public RegionManager()
        {
            ConnectionString= string.Format("server=10.22.102.90;user id=root;passsword=Ztop123456;database=intensiveuse");
        }
        public List<string> GetProvonces()
        {
            //var list = new List<string>();
            //using (var connection=new MySqlConnection(ConnectionString))
            //{
            //    connection.Open();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = "select Province from region group by Province";
            //        var reader = command.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            var str = reader[0].ToString();
            //            if (!string.IsNullOrEmpty(str))
            //            {
            //                list.Add(str);
            //            }
            //        }
            //    }

            //    connection.Close();
            //}
            //return list;
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.GroupBy(e => e.Province).Select(g => g.Key).ToList();
            }
        }
        public List<string> GetCitys(string province)
        {
            if (string.IsNullOrEmpty(province))
            {
                return null;
            }
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.Where(e => e.Province == province.Trim()).GroupBy(e => e.BelongCity).Select(g => g.Key).ToList();
            }
        }
        public List<string> GetCounty(string province,string belongCity)
        {
            if (string.IsNullOrEmpty(province) || string.IsNullOrEmpty(belongCity))
            {
                return null;
            }
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.Where(e => e.Province == province.Trim() && e.BelongCity == belongCity.Trim()).GroupBy(e => e.Name).Select(g => g.Key).ToList();
            }
        }
        public int GetID(string province,string belongCity,string name=null)
        {
            var region = Get(province, belongCity, name);
            if (region != null)
            {
                return region.ID;
            }
            return 0;
        }
        public Region Get(string province,string belongCity,string name=null)
        {
            using (var db = GetIntensiveUseContext())
            {
                if (string.IsNullOrEmpty(name))
                {
                    return db.Regions.FirstOrDefault(e => e.Province == province.Trim() && e.BelongCity == belongCity.Trim() && e.Name==belongCity.Trim());
                }
                else
                {
                    return db.Regions.FirstOrDefault(e => e.Province == province.Trim() && e.BelongCity == belongCity.Trim() && e.Name == name.Trim());
                }
            }
        }
    }
}