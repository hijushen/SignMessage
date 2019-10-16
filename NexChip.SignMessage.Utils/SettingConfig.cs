using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public class SettingConfig
    {
        public static string PostUrl = AppConfigManager.Configuration["POSTURL"];

        public static string ApiTokenString = AppConfigManager.Configuration["ApiTokenString"];


        public static string AllString =  AppConfigManager.Configuration["AllString"];
        public static string NoBroGTitle = AppConfigManager.Configuration["NoBroGTitle"];


        public static string UnReadRefreshSeconds = AppConfigManager.Configuration["UnReadRefreshSeconds"];

        public static string SignMessageBoxPartShow = AppConfigManager.Configuration["SignMessageBoxPartShow"];
        public static string BrogSignMessageUrl = AppConfigManager.Configuration["BrogSignMessageUrl"];

        public static string NewOAUrl = AppConfigManager.Configuration["NewOAUrl"];
        public static string NewOACheck = AppConfigManager.Configuration["NewOACheck"];
    }
}
