using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class BizListResult<T>
    {
        /// <summary>
        /// 表格数据，支持分页
        /// </summary>
        public int Code { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<T> Data { get; set; }
    }
}
