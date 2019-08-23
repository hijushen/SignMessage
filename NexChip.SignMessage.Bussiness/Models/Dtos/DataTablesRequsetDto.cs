using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NexChip.SignMessage.Bussiness.Models.Dtos
{
    public class DataTablesRequsetDto
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string search {get;set;}

        public string orderString { get; set; }


        public string extra_search { get; set; }
    }
}
