using System;
using System.Collections.Generic;
using System.Text;

namespace Luyao.SuperTools.Cache
{
    /// <summary>
    /// 缓存字典的值
    /// </summary>
    public class MDicValue
    {
        public DateTime ExpiredTime { get; set;  }

        public object Value { get; set; }
    }
}
