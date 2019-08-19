using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexChip.SignMessage.Web.Models.Dtos
{
    public class CommonListDto
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
        /// 
        /// </summary>
        public string sortfiled { get; set; }

        public string sortway { get; set; }
    }

    public class SignMessageBoxDto : CommonListDto
    {

        public string OID { get; set; }
       
        public string appname { get; set; }
       
        public string msgsourceid { get; set; }
       
        public int msgstatus { get; set; }
         
        public int emergencylevel { get; set; }
         
        public DateTime? sendtime { get; set; }
        
        public string fromempid { get; set; }
         
        public string fromempname { get; set; }
      
        public string toempid { get; set; }
        
        public string toempname { get; set; }
           
        public string callbackurl { get; set; }
       
        public string substance { get; set; }
         
        public string showmsg { get; set; }
      
        public DateTime createtime { get; set; }
    
        public DateTime updatetime { get; set; }
      
        public string remark { get; set; }
    }
}
