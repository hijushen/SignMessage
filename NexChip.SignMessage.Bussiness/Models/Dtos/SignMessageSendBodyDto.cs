using Newtonsoft.Json;
using System;

namespace NexChip.SignMessage.Bussiness
{
    /// <summary>
    /// 签核箱消息内容实体
    /// </summary>
    public class SignMessageSendBodyDto
    {
        /// <summary>
        /// 原系统id
        /// </summary>
        public string sourceid { get; set; }

        /// <summary>
        /// 发送人id
        /// </summary>
        public string fromid { get; set; }

        /// <summary>
        /// 发送人名称
        /// </summary>
        public string fromname { get; set; }

        /// <summary>
        /// 接收方ids
        /// </summary>
        public string toids { get; set; }

        /// <summary>
        /// 接收方名字
        /// </summary>
        public string tonames { get; set; }

        /// <summary>
        /// 1.新增 2.完成 3.取消
        /// </summary>
        public int handletype { get; set; }

        /// <summary>
        /// 跳转回 业务系统 url
        /// </summary>
        public string callbackurl { get; set; }

        /// <summary>
        /// 紧急程度1.平件 2.急件 3.特急（默认1）
        /// </summary>
        public int? emergencylevel { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string substance { get; set; }

        /// <summary>
        /// 需要额外显示在签核箱中的内容
        /// </summary>
        public string showmsg { get; set; }


        /// <summary>
        /// 发送时间
        /// <example>2019-08-31 19:00:00</example>
        /// </summary>
        //[JsonConverter(typeof(CustomDateConverter))]
        public DateTime? createtime { get; set; }

        /// <summary>
        /// 发送时间
        /// <example>2019-08-31 19:00:00</example>
        /// </summary>
        //[JsonConverter(typeof(CustomDateConverter))]
        public DateTime? updatetime { get; set; }

        /// <summary>
        /// 发送消息原系统id
        /// </summary>
        public string msgsourceid { get; set; }
    }
}