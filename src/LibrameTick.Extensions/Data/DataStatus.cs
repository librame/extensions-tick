#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据状态。
    /// </summary>
    [Description("数据状态")]
    public enum DataStatus
    {

        #region Global

        /// <summary>
        /// 默认。
        /// </summary>
        [Display(Name = nameof(Default), GroupName = "GlobalGroup", ResourceType = typeof(DataStatusResource))]
        Default = 1,

        /// <summary>
        /// 删除。
        /// </summary>
        [Display(Name = nameof(Delete), GroupName = "GlobalGroup", ResourceType = typeof(DataStatusResource))]
        Delete = 2,

        #endregion


        #region Scope

        /// <summary>
        /// 公开。
        /// </summary>
        [Display(Name = nameof(Public), GroupName = "ScopeGroup", ResourceType = typeof(DataStatusResource))]
        Public = 4,

        /// <summary>
        /// 保护。
        /// </summary>
        [Display(Name = nameof(Protect), GroupName = "ScopeGroup", ResourceType = typeof(DataStatusResource))]
        Protect = 8,

        /// <summary>
        /// 内部。
        /// </summary>
        [Display(Name = nameof(Internal), GroupName = "ScopeGroup", ResourceType = typeof(DataStatusResource))]
        Internal = 16,

        /// <summary>
        /// 私有。
        /// </summary>
        [Display(Name = nameof(Private), GroupName = "ScopeGroup", ResourceType = typeof(DataStatusResource))]
        Private = 32,

        #endregion


        #region State

        /// <summary>
        /// 活动。
        /// </summary>
        [Display(Name = nameof(Active), GroupName = "StateGroup", ResourceType = typeof(DataStatusResource))]
        Active = 64,

        /// <summary>
        /// 挂起。
        /// </summary>
        [Display(Name = nameof(Pending), GroupName = "StateGroup", ResourceType = typeof(DataStatusResource))]
        Pending = 128,

        /// <summary>
        /// 闲置。
        /// </summary>
        [Display(Name = nameof(Inactive), GroupName = "StateGroup", ResourceType = typeof(DataStatusResource))]
        Inactive = 256,

        /// <summary>
        /// 锁定。
        /// </summary>
        [Display(Name = nameof(Locking), GroupName = "StateGroup", ResourceType = typeof(DataStatusResource))]
        Locking = 512,

        /// <summary>
        /// 禁止。
        /// </summary>
        [Display(Name = nameof(Ban), GroupName = "StateGroup", ResourceType = typeof(DataStatusResource))]
        Ban = 1024,

        /// <summary>
        /// 废弃。
        /// </summary>
        [Display(Name = nameof(Obsolete), GroupName = "StateGroup", ResourceType = typeof(DataStatusResource))]
        Obsolete = 2048
        
        #endregion
        
    }
}