#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Saving;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.Device;
using Librame.Extensions.IdGeneration;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="IExtensionOptions"/> 的数据扩展选项。
/// </summary>
public class DataExtensionOptions : AbstractExtensionOptions<DataExtensionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="DataExtensionOptions"/>。
    /// </summary>
    public DataExtensionOptions()
    {
        ShardingDirectory = Directories.ResourceDirectory.CombineDirectory("shardings");
    }


    /// <summary>
    /// 分片目录。
    /// </summary>
    public string ShardingDirectory { get; set; }


    /// <summary>
    /// 访问选项。
    /// </summary>
    public AccessOptions Access { get; set; } = new();

    /// <summary>
    /// 审计选项。
    /// </summary>
    public AuditOptions Audit { get; set; } = new();

    /// <summary>
    /// 数据标识生成选项。
    /// </summary>
    public DataIdGenerationOptions DataIdGeneration { get; set; } = new();

    /// <summary>
    /// 分片选项。
    /// </summary>
    public ShardingOptions Sharding { get; set; } = new();

    /// <summary>
    /// 分片设置选项。
    /// </summary>
    public ShardingSettingOptions Setting { get; set; } = new();

    /// <summary>
    /// 存储选项。
    /// </summary>
    public StoreOptions Store { get; set; } = new();

    /// <summary>
    /// 设备负载选项。
    /// </summary>
    public DeviceLoadOptions DeviceLoad { get; set; } = new();

    /// <summary>
    /// 设备负载每次实时计算（如果不启用，当首次等待计算后，下次先返回上次计算值，利用率将在后台计算后更新，以提升响应速度；默认不启用）。
    /// </summary>
    public bool DeviceLoadRealtimeForEverytime { get; set; }

    /// <summary>
    /// 存取器设备负载器工厂（默认使用 <see cref="AccessorDeviceLoader"/>）。
    /// </summary>
    [JsonIgnore]
    public Func<DataExtensionOptions, IEnumerable<string>, IDeviceLoader> DeviceLoaderFactory { get; set; }
        = (options, hosts) => new AccessorDeviceLoader(options, hosts);

    /// <summary>
    /// 保存变化上下文工厂（默认使用 <see cref="SavingChangesContext"/>）。
    /// </summary>
    [JsonIgnore]
    public Func<DataContext, ISavingChangesContext> SavingChangesContextFactory { get; set; }
        = context => new SavingChangesContext(context);

    /// <summary>
    /// 后置创建模型的配置实体类型动作。
    /// </summary>
    public Action<DataContext, ModelBuilder, IMutableEntityType>? PostConfigureEntityTypeAction { get; set; }

    /// <summary>
    /// 后置创建模型的配置实体类型属性动作。
    /// </summary>
    public Action<DataContext, ModelBuilder, IMutableEntityType, PropertyInfo>? PostConfigureEntityTypePropertyAction { get; set; }

    /// <summary>
    /// 查询过滤器集合。
    /// </summary>
    [JsonIgnore]
    public List<IQueryFilter> QueryFilters { get; init; } = [];

    /// <summary>
    /// 保存变化的处理程序集合。
    /// </summary>
    [JsonIgnore]
    public List<ISavingChangesHandler> SavingChangesHandlers { get; init; } = [];

    /// <summary>
    /// 保存审计集合动作（默认保存到当前数据库）。
    /// </summary>
    [JsonIgnore]
    public Action<DataContext, IEnumerable<Audit>> SavingAuditsAction { get; set; }
        = (context, audits) => context.Set<Audit>().AddRange(audits);

}
