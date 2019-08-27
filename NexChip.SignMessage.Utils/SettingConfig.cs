using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public class SettingConfig
    {
        public static string PostUrl = AppConfigManager.Configuration["POSTURL"];

        public static string ApiTokenString = AppConfigManager.Configuration["ApiTokenString"];
    }
}
