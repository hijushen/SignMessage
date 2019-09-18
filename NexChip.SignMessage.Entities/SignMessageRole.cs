using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace NexChip.SignMessage.Entities
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("SignMessageRole")]
    public partial class SignMessageRole
    {
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string OID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string appname {get;set;}


        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string appnamechs { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string reservedkey1 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string reservedkey2 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:1
           /// Nullable:False
           /// </summary>           
           public int rolestatus {get;set;}

        /// <summary>
        /// 是否展示
        /// </summary>
           public int isshow { get; set; }

           /// <summary>
           /// Desc:
           /// Default:dateadd(year,(10),getdate())
           /// Nullable:False
           /// </summary>           
           public DateTime invalidtime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:DateTime.Now
           /// Nullable:False
           /// </summary>           
           public DateTime createtime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:DateTime.Now
           /// Nullable:False
           /// </summary>           
           public DateTime updatetime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string creater {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string updater {get;set;}

    }
}
