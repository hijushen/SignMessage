using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NexChip.SignMessage.Bussiness.Models.Dtos
{
    public class DataTablesRequsetDto
    {
        public string logonid { get; set; }

        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string search {get;set;}

        public string orderString { get; set; }

        /// <summary>
        /// 日期区间
        /// </summary>
        public string timespan { get; set; }

        /// <summary>
        /// 表单类型
        /// </summary>
        public string formtype { get; set; }

        /// <summary>
        /// 处理状况
        /// </summary>
        public string handlestatus { get; set; }
    }
}
