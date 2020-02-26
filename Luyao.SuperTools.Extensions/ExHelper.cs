using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Luyao.SuperTools.Extensions
{
    /// <summary>
    /// 扩展帮助类
    /// 无异常处理
    /// </summary>
    public static class ExHelper
    {
        /// <summary>
        /// 向控制台输出-自带时间
        /// </summary>
        /// <param name="str"></param>
        public static void ConsoleWriteLine(this string str)
        {
            Console.WriteLine($"{DateTime.Now} {str}");
        }

        /// <summary>
        /// 向控制台输出-自带时间
        /// 带错误日志
        /// </summary>
        /// <param name="str"></param>
        public static void ConsoleWriteLineWithErrLog(this string str)
        {
            Console.WriteLine($"{DateTime.Now} {str}");
            throw new Exception($"方法未实现");
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(this T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T SerializeObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 获取字符串中对应类型的字符
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="type">字符类型(中文,英文....)</param>
        /// <param name="middle">每个字符用这该符号隔开(默认空) 例如 123abc3d 找英文 用-隔开  结果为:abc-d</param>
        /// <returns>删选完后的字符串</returns>
        public static string GetLanguages(this string str, EnumLanguage type,string middle="")
        {
            Regex reg;
            switch (type)
            {
                case EnumLanguage.中文:
                    reg = new Regex("[\u4e00-\u9fa5]+");
                    break;
                case EnumLanguage.数字:
                    reg = new Regex("[0-9]");
                    break;
                case EnumLanguage.字母:
                    reg = new Regex("^[A-Za-z]+$");
                    break;
                case EnumLanguage.特殊符号:
                    reg = new Regex("[\u4e00-\u9fa5]+");
                    break;
                default:
                    throw new Exception($"未识别的枚举类型。type:{type.ToString()}");
            }

            string result = string.Empty;
            foreach (var item in reg.Matches(str))
            {
                result += item.ToString();
                result += middle;
            }

            return result.Substring(0, result.Length- middle.Length);
        }
    }
}
