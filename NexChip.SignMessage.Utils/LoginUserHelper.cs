
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public class LoginUserHelper
    {
        public static string GetLoginUserName(string logonid, string identityName)
        {
            if(logonid == null)
            {
                string[] domains = identityName.Split("\\");
                if(domains.Length > 1)
                {
                    return domains[domains.Length - 1];
                }
                else
                {
                    return domains[0];
                }
            }
            else//切换模式，地址栏自带名称
            {
                return logonid;
            }
        }

        public class ValiResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
        }

//        public static ValiResult valiUser(string logonid,string sHAEncry)
//        {
//            ValiResult res = new ValiResult() { Success=false};
//            if (string.IsNullOrWhiteSpace(logonid))
//            {
//                res.Success = true;
//                return res;
//            }

//            try
//            {
//                var ass = Assembly.LoadFile("NewOA_JH.dll");//我们要调用的dll或exe文件路径
//                var tp = ass.GetType("namepace.class"); //获取类名，必须 命名空间+类名
//                var  obj = Activator.CreateInstance(tp); //建立实例
//                var odInfo meth = tp.GetMethod("mymethod"); //获取要调用的方法
//.Invoke(obj, new string[] { "args1", "args2" });//Invoke调用方法,"{}"里为方法需<span style="font-family: Arial, Helvetica, sans-serif;">要的参数</

               

//                Encryption objPscSHA = new Encryption();
//                var retSHA = Encryption.AccessEform(logonid, sHAEncry + string.Empty);
//                if(retSHA == 1)
//                {
//                    res.Success = true;
//                    return res;
//                }

//                switch (retSHA)
//                {
//                    case 0: res.Msg = "资料验证错误，请重新开启签核箱！"; break;
//                    case -1: res.Msg = "网页网址已经过时，请重新开启签核箱！"; break;
//                    case -2: res.Msg = "无法取得登入帐号，请重新开启签核箱！"; break;
//                    case -3: res.Msg = "身分验证错误，请重新开启签核箱！"; break;
//                    case -4: res.Msg = "身分验证错误(Hash)，请重新开启签核箱！"; break;
//                    case -5: res.Msg = "对不起！您不是合法管理人员，请由晶合Portal切换身分开启签核箱，谢谢。"; break;
//                    default: res.Msg = "资料验证其他错误，请重新开启签核箱"; break;
//                }              

//            }
//            catch(Exception ex)
//            {
//                res.Msg = "资料验证其他错误，请重新开启签核箱" + ex.Message;
//                return res;
//            }

//            return res;
//        }
    }
}
