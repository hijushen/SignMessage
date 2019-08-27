using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public static class DesEncryptTool
    {
        public static string EncryptStr(this string content, string desKey)
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(desKey))
                {
                    result = content;
                }
                else
                {
                    DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                    dESCryptoServiceProvider.Mode = CipherMode.ECB;
                    byte[] array = new byte[8];
                    if (desKey.Length < 8)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(desKey);
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (bytes.Length > i)
                            {
                                array[i] = bytes[i];
                            }
                            else
                            {
                                array[i] = 0;
                            }
                        }
                    }
                    else
                    {
                        array = Encoding.UTF8.GetBytes(desKey.Substring(0, 8));
                    }
                    dESCryptoServiceProvider.Key = array;
                    dESCryptoServiceProvider.IV = dESCryptoServiceProvider.Key;
                    byte[] bytes2 = Encoding.UTF8.GetBytes(content);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(bytes2, 0, bytes2.Length);
                    cryptoStream.FlushFinalBlock();
                    StringBuilder stringBuilder = new StringBuilder();
                    byte[] array2 = memoryStream.ToArray();
                    for (int j = 0; j < array2.Length; j++)
                    {
                        byte b = array2[j];
                        stringBuilder.Append(b.ToString());
                        stringBuilder.Append("_");
                    }
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    }
                    result = stringBuilder.ToString();
                }
            }
            catch (Exception)
            {
                result = content;
            }
            return result;
        }

        public static string DecryptStr(this string content, string desKey)
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(desKey))
                {
                    result = content;
                }
                else
                {
                    DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                    dESCryptoServiceProvider.Mode = CipherMode.ECB;
                    byte[] array = new byte[8];
                    if (desKey.Length < 8)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(desKey);
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (bytes.Length > i)
                            {
                                array[i] = bytes[i];
                            }
                            else
                            {
                                array[i] = 0;
                            }
                        }
                    }
                    else
                    {
                        array = Encoding.UTF8.GetBytes(desKey.Substring(0, 8));
                    }
                    dESCryptoServiceProvider.Key = array;
                    dESCryptoServiceProvider.IV = dESCryptoServiceProvider.Key;
                    string[] array2 = content.Split(new char[]
                    {
                '_'
                    });
                    byte[] array3 = new byte[array2.Length];
                    for (int j = 0; j < array2.Length; j++)
                    {
                        array3[j] = Convert.ToByte(array2[j]);
                    }
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(array3, 0, array3.Length);
                    cryptoStream.FlushFinalBlock();
                    Encoding encoding = new UTF8Encoding();
                    result = encoding.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }
    }
}
