#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义多访问器的访问关系。
    /// </summary>
    public enum AccessorsRelationship
    {
        /// <summary>
        /// 对多访问器实现的聚合访问。在此设定下，可以对多访问器进行镜像操作。
        /// </summary>
        Aggregation,

        /// <summary>
        /// 对多访问器实现的切片访问。在此模式下，可以对多访问器进行分库操作。
        /// </summary>
        Slicing
    }
}
