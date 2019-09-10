using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace NexChip.SignMessage.Entities
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("SignMessageInterface")]
    public partial class SignMessageInterface
    {
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string OID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string appname {get;set;}


        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime sendtime { get; set; }


        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int? handleresult {get;set;}

        /// <summary>
        /// 处理错误提示
        /// </summary>
          public string handleerrormsg { get; set; }

        /// <summary>
        /// 处理完消息OIDs （单条或者多条）
        /// </summary>
        public string handlemsgoids { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? createtime { get; set; }


        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? updatetime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string msgbody {get;set;}

    }
}
