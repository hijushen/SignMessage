using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexChip.SignMessage.Bussiness.Models.Dtos
{
    public class CommonPagingDto
    {
        /// <summary>
        /// start 起始
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sortfiled { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string sortway { get; set; }
    }
}
