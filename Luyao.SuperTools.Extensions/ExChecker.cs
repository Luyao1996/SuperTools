using System;

namespace Luyao.SuperTools.Extensions
{
    /// <summary>
    /// 扩展校验类
    /// 无异常处理
    /// </summary>
    public static class ExChecker
    {
        /// <summary>
        /// 判断string是否为null或empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns>bool</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断string是否为null或者空格或者empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns>bool<returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
