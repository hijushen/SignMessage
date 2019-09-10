using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class BizListResultForDataTables<T>
    {
        public int draw { get; set; }

        //总数
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

        public List<T> data { get; set; }

        public string error { get; set; }

        public int unReadCount { get; set; }
    }
}
