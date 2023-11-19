#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

public interface IShardingFinder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="contextType"></param>
    /// <returns></returns>
    IReadOnlyList<ShardingAttribute> Find(Type contextType);
}
