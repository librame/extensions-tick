#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="IMutableAnnotatable"/> 静态扩展。
/// </summary>
public static class MutableAnnotatableExtensions
{
    ///// <summary>
    ///// 初始表注释名称。
    ///// </summary>
    //public const string InitialTableAnnotationName = RelationalAnnotationNames.Prefix + "InitialTableName";

    ///// <summary>
    ///// 可变模型。
    ///// </summary>
    //public const string MutableModelAnnotationName = "MutableModel";

    /// <summary>
    /// 分片注释名称。
    /// </summary>
    public const string ShardingBuilderAnnotationName = RelationalAnnotationNames.Prefix + "ShardingBuilder";


    ///// <summary>
    ///// 获取初始表名。
    ///// </summary>
    ///// <param name="annotatable">给定的 <see cref="IMutableAnnotatable"/>。</param>
    ///// <returns>返回表名字符串。</returns>
    //public static string? GetInitialTableName(this IMutableAnnotatable annotatable)
    //    => annotatable.FindAnnotation(InitialTableAnnotationName)?.Value?.ToString();

    ///// <summary>
    ///// 设置初始表名。
    ///// </summary>
    ///// <param name="annotatable">给定的 <see cref="IMutableAnnotatable"/>。</param>
    ///// <param name="initialTableName">给定的初始表名。</param>
    //public static void SetInitialTableName(this IMutableAnnotatable annotatable, string initialTableName)
    //    => annotatable.SetAnnotation(InitialTableAnnotationName, initialTableName);


    ///// <summary>
    ///// 获取可变模型。
    ///// </summary>
    ///// <param name="model">给定的 <see cref="IModel"/>。</param>
    ///// <returns>返回 <see cref="Model"/>。</returns>
    //public static Model? GetMutableModel(this IModel model)
    //    => (Model?)model.FindRuntimeAnnotationValue(MutableModelAnnotationName);

    ///// <summary>
    ///// 设置可变模型。
    ///// </summary>
    ///// <param name="model">给定的 <see cref="IModel"/>。</param>
    ///// <param name="mutableModel">给定的可变 <see cref="Model"/>。</param>
    //public static void SetMutableModel(this IModel model, Model mutableModel)
    //    => model.AddRuntimeAnnotation(MutableModelAnnotationName, mutableModel);


    /// <summary>
    /// 获取实体类型配置的分片描述符。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
    /// <param name="strategyProvider">给定的 <see cref="IShardingStrategyProvider"/>。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
    public static ShardingDescriptor? GetShardingDescriptor(this IEntityType entityType, IShardingStrategyProvider strategyProvider)
    {
        var shardingAnnotation = entityType.FindAnnotation(ShardingBuilderAnnotationName);
        if (shardingAnnotation?.Value is not null)
        {
            var methodName = nameof(ShardingEntityTypeBuilder<ShardingDescriptor>.CreateDescriptor);

            var method = shardingAnnotation.Value.GetType().GetMethod(methodName, TypeExtensions.NonPublicMemberFlags);

            return (ShardingDescriptor?)method?.Invoke(shardingAnnotation.Value, [strategyProvider]);
        }

        var attribute = entityType.GetShardingAttribute();
        if (attribute is null) return null;

        return new ShardingDescriptor(strategyProvider, attribute);
    }


    /// <summary>
    /// 获取分片实体类型构建器。
    /// </summary>
    /// <param name="annotatable">给定的 <see cref="IMutableAnnotatable"/>。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public static ShardingEntityTypeBuilder<TEntity>? GetShardingBuilder<TEntity>(this IMutableAnnotatable annotatable)
        where TEntity : class
        => (ShardingEntityTypeBuilder<TEntity>?)annotatable.FindAnnotation(ShardingBuilderAnnotationName)?.Value;

    /// <summary>
    /// 设置分片实体类型构建器。
    /// </summary>
    /// <param name="annotatable">给定的 <see cref="IMutableAnnotatable"/>。</param>
    /// <param name="value">给定的 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</param>
    public static void SetShardingBuilder<TEntity>(this IMutableAnnotatable annotatable, ShardingEntityTypeBuilder<TEntity> value)
        where TEntity : class
        => annotatable.SetAnnotation(ShardingBuilderAnnotationName, value);

}
