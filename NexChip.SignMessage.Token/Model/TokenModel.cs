using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Token
{
    public class TokenModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Uid { get; set; }



        /// <summary>
        /// 程序名
        /// </summary>
        public string AppName { get; set; }


        /// <summary>
        /// 程序中文名
        /// </summary>
        public string AppNameCHS { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project { get; set; }

        public DateTime expireTime { get; set; }
    }
}
