using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Services
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class BizResult<T>
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
