using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Luyao.SuperTools.Extensions;

namespace Luyao.SuperTools.IOHelper
{
    public class ConfigHelper
    {
        private static IConfigurationRoot File { get; set; }

        /// <summary>
        /// 配置文件名列表
        /// 不配置时默认appsettings
        /// </summary>
        public static List<string> JsonFileNames { get; set; } = new List<string>();

        /// <summary>
        /// 配置文件名
        /// 不配置时默认appsettings
        /// </summary>
        public static string JsonFileName { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        private static void Initialization()
        {
            if (JsonFileName.IsNullOrEmpty() && JsonFileNames.Count == 0)
                JsonFileName = "appsettings.json";
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

                JsonFileNames.Add(JsonFileName);
                foreach (var item in JsonFileNames)
                {
                    builder.AddJsonFile(item);
                }

                File = builder.Build();
            }
            catch (Exception ex)
            {
                throw new Exception($"初始化异常:{ex.Message}");
            }
            
        }

        /// <summary>
        /// 获取指定节点信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static T Get<T>(string nodeName)
        {
            if (File == null)
            {
                Initialization();
            }

            if (nodeName.IsNullOrEmpty())
                return default(T);

            try
            {
                return (T)Convert.ChangeType(File[nodeName],typeof(T));
            }
            catch
            {
                throw new Exception($"转换类型失败!获取到的值为:{File[nodeName]}");
            }
        }

        /// <summary>
        /// 获取指定对象的子级节点信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodeName">对象名称：子级节点名称 || 对象名称：子级对象名称：子级节点名称</param>
        /// <returns></returns>
        public static T GetSection<T>(string section)
        {
            if (File == null)
            {
                Initialization();
            }

            if (section.IsNullOrEmpty())
                return default(T);

            try
            {
                return (T)Convert.ChangeType(File.GetSection(section).Value, typeof(T));
            }
            catch
            {
                throw new Exception($"转换类型失败!获取到的值为:{File.GetSection(section).Value}");
            }
        }
    }
}
