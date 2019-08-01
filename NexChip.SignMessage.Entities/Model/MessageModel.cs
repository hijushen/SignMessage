using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Entities
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class MessageModel<T>
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public List<T> Data { get; set; }
    }
}
