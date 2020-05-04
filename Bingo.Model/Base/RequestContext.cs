using Bingo.Dao.BingoDb.Entity;
using System;
using System.Collections.Generic;

namespace Bingo.Model.Base
{
    public class RequestContext<T>
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public RequestHead Head { get; set; }

        /// <summary>
        /// 请求体
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public RequestContext()
        {
            Head = new RequestHead();
            Data = default;
        }
    }

    public class RequestHead
    {
        public RequestHead()
        {
            TransactionId = Guid.NewGuid();
        }

        /// <summary>
        /// Token信息
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UId { get; set; }

        /// <summary>
        /// 纬度，范围为 -90~90，负数表示南纬
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度，范围为 -180~180，负数表示西经
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 事务号
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// 拓展信息
        /// </summary>
        public List<KeyValue> Extensions;
    }
}
