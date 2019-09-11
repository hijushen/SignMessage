using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness.Enum
{
    public class HandleStatusString
    {
        public static string Undo { get; } = "未完成";
        public static string Done { get; } = "已完成";
        public static string Del { get; set; } = "已删除";
    }
}
