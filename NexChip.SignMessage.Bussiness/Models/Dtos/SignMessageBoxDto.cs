using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexChip.SignMessage.Bussiness.Models.Dtos
{
    public class SignMessageBoxDto : CommonPagingDto
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
