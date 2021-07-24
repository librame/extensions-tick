#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义用于复合的 <see cref="IAccessor"/>。
    /// </summary>
    public sealed class CompositeAccessor : IAccessor
    {
        private readonly IAccessor[] _accessors;


        /// <summary>
        /// 构造一个 <see cref="CompositeAccessor"/>。
        /// </summary>
        /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
        public CompositeAccessor(IEnumerable<IAccessor> accessors)
        {
            accessors.NotNull(nameof(accessors));

            _accessors = accessors.ToArray();
        }


        /// <summary>
        /// 访问器类型。
        /// </summary>
        public Type AccessorType
            => typeof(CompositeAccessor);


        #region ISortable

        /// <summary>
        /// 访问器优先级。
        /// </summary>
        public float Priority
            => 0;


        /// <summary>
        /// 与指定的 <see cref="ISortable"/> 比较大小。
        /// </summary>
        /// <param name="other">给定的 <see cref="ISortable"/>。</param>
        /// <returns>返回整数。</returns>
        public int CompareTo(ISortable? other)
            => Priority.CompareTo(other?.Priority ?? 0);

        #endregion


        //public InterceptionResult ConnectionOpening(
        //    DbConnection connection,
        //    ConnectionEventData eventData,
        //    InterceptionResult result)
        //{
        //    for (var i = 0; i < _accessors.Length; i++)
        //    {
        //        result = _accessors[i].ConnectionOpening(connection, eventData, result);
        //    }

        //    return result;
        //}

    }
}
