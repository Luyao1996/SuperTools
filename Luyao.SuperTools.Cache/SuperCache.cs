using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Luyao.SuperTools.Extensions;

namespace Luyao.SuperTools.Cache
{
    /// <summary>
    /// 简单字典实现
    /// </summary>
    public class SuperCache
    {
        /// <summary>
        /// 线程安全的字典缓存
        /// </summary>
        private static ConcurrentDictionary<string, MDicValue> DicCache {get;set;}

        /// <summary>
        /// 过期的key列表
        /// </summary>
        private static List<string> ListDelKeys { get; set; }

        /// <summary>
        /// 设置自动过期检查时间(单位:秒 默认:30分钟)
        /// 可实时更新
        /// </summary>
        public static int SleepSecond { get; set; }

        

        /// <summary>
        /// 初始化字典|主动过期
        /// </summary>
        static SuperCache(){
            DicCache = new ConcurrentDictionary<string, MDicValue>();
            TaskCacheExpired();
        }

        /// <summary>
        /// 判断是否存在key
        /// </summary>
        /// <param name="key"></param>
        public static bool Exsit(string key)
        {
            return DicCache.ContainsKey(key);
        }

        #region 增删查

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>bool</returns>
        public static bool Add(string key, MDicValue value)
        {
            return DicCache.TryAdd(key, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>bool</returns>
        public static bool Del(string key)
        {
            return DicCache.TryRemove(key, out MDicValue value);
        }

        /// <summary>
        /// 根据表达式删除缓存
        /// 返回值为已删除的键
        /// 未完成删除的键有日志记录与控制台输出
        /// </summary>
        /// <param name="func">委托:返回值为要删除的键的特征(包含该特征的所有键都会被删除)</param>
        /// <returns>已删除的键</returns>
        public static List<string> DelFunc(Func<string> func)
        {
            List<string> listRemoved = new List<string>();
            List<string> listRemove = new List<string>();
            string strDel = func.Invoke();

            foreach (var item in DicCache.Keys)
            {
                if (item.Contains(strDel))
                {
                    listRemove.Add(item);
                }
            }

            listRemove.ForEach(key=> {
                try
                {
                    bool flag = Del(key);
                    if (flag)
                        throw new Exception($"删除失败！");

                    listRemoved.Add(key);
                }
                catch (Exception ex)
                {
                    $"键:{key} 删除时发生异常:{ex.Message}".ConsoleWriteLineWithErrLog();
                }
            });

            return listRemoved;
        }

        /// <summary>
        /// 查询缓存数据
        /// 若存在则直接返回
        /// 若不存在则新增一个
        /// </summary>
        /// <param name="key">键 若该键不存在对应的值,则该键位新增入缓存的键</param>
        /// <param name="func">委托:返回值为要新增的缓存的值</param>
        /// <returns></returns>
        public static MDicValue Find(string key, Func<MDicValue> func)
        {
            MDicValue findValue = Find(key);
            if (findValue==null)
            {
                MDicValue value = func.Invoke();
                if (!Add(key, value))  
                    return null;
            }
            return findValue;
        }

        /// <summary>
        /// 查询缓存数据
        /// 若存在则返回值
        /// 若不存在则返回null
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static MDicValue Find(string key)
        {
            bool flag = Exsit(key);

            //是否存在
            if (!flag)
                return null;

            //是否过期
            if (DicCache[key].ExpiredTime > DateTime.Now)
            {
                Del(key);
                return null;
            }

            return DicCache[key];
        }

        #endregion

        /// <summary>
        /// 自动过期
        /// </summary>
        private static void TaskCacheExpired()
        {
            Task.Run(()=> {
                while (true)
                {
                    try
                    {
                        $"自动过期线程开始运行!".ConsoleWriteLine();

                        ListDelKeys = new List<string>();
                        foreach (var item in DicCache.Keys)
                        {
                            bool flag = DicCache[item].ExpiredTime > DateTime.Now;
                            if (flag)
                                ListDelKeys.Add(item);
                        }

                        ListDelKeys.ForEach(key=> {
                            bool flag= DicCache.TryRemove(key, out MDicValue tempModel);
                            if (!flag)
                                $"从字典中删除键：{key}失败！TryRemove的返回值为:{tempModel.SerializeObject()}".ConsoleWriteLineWithErrLog();
                        });


                        ListDelKeys = null;

                    }
                    catch (Exception ex)
                    {
                        $"自动过期线程发生异常:{ex.Message}".ConsoleWriteLineWithErrLog();
                    }
                    finally
                    {
                        SleepSecond = SleepSecond == 0 ? 15 * 60 : SleepSecond;
                        $"自动过期线程开始休眠,休眠时间:{SleepSecond}秒，下次运行时间:{DateTime.Now.AddSeconds(SleepSecond)}".ConsoleWriteLine();
                        Thread.Sleep( SleepSecond * 1000 );
                    }
                }
            });
        }
    }
}
