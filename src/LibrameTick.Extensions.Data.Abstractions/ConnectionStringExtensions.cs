#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 数据库连接字符串静态扩展。
/// </summary>
public static class ConnectionStringExtensions
{
    /// <summary>
    /// 默认已支持的数据库键集合。
    /// </summary>
    public static readonly string[] DefaultSupportedDatabaseKeys
        = new string[] { "Database", "Initial Catalog", "Data Source" };


    /// <summary>
    /// 从数据库连接字符串解析数据库名。
    /// </summary>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <returns>返回数据库名。</returns>
    /// <exception cref="ArgumentException">
    /// A matching supported database keys was not found from the current connection string, Please specify the database key.
    /// </exception>
    public static string ParseDatabaseFromConnectionString(this string? connectionString)
        => connectionString.ParseDatabaseFromConnectionString(out var _, out var _);

    /// <summary>
    /// 从数据库连接字符串解析数据库名。
    /// </summary>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <param name="segments">输出连接字符串的键值对集合。</param>
    /// <param name="isFileDatabase">输出是否为文件型数据源。</param>
    /// <param name="keyValueSeparator">给定的键值对分隔符（可选；默认为等号）。</param>
    /// <param name="pairDelimiter">给定的键值对集合界定符（可选；默认为分号）。</param>
    /// <param name="databaseKey">给定的数据库键（可选；默认使用 <see cref="DefaultSupportedDatabaseKeys"/>）。</param>
    /// <returns>返回数据库名。</returns>
    /// <exception cref="ArgumentException">
    /// A matching supported database keys was not found from the current connection string, Please specify the database key.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The database key for the current connection string is null or empty.
    /// </exception>
    public static string ParseDatabaseFromConnectionString(this string? connectionString,
        out Dictionary<string, string>? segments, out bool isFileDatabase,
        string keyValueSeparator = "=", string pairDelimiter = ";", string? databaseKey = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        // 提取连接字符串的键值对集合
        segments = connectionString
            .TrimEnd(pairDelimiter)
            .Split(pairDelimiter)
            .Select(part =>
            {
                var pairPart = part.Split(keyValueSeparator);
                return new KeyValuePair<string, string>(pairPart[0], pairPart[pairPart.Length - 1]);
            })
            .ToDictionary(ks => ks.Key, ele => ele.Value, StringComparer.OrdinalIgnoreCase);

        var database = string.Empty;

        // 解析数据库键值
        if (string.IsNullOrEmpty(databaseKey))
        {
            foreach (var key in DefaultSupportedDatabaseKeys)
            {
                if (segments.TryGetValue(key, out var value))
                {
                    databaseKey = key;
                    database = value;

                    break;
                }
            }

            if (string.IsNullOrEmpty(databaseKey))
                throw new ArgumentException($"A matching supported database keys '{DefaultSupportedDatabaseKeys.JoinString(',')}' was not found from the current connection string '{connectionString}', Please specify the database key.");
        }
        else
        {
            database = segments[databaseKey];
        }

        if (string.IsNullOrEmpty(database))
            throw new ArgumentException($"The database key '{databaseKey}' for the current connection string '{connectionString}' is null or empty.");

        // 修剪可能存在的路径和文件扩展名
        if (database.Contains('.') || database.Contains(Path.DirectorySeparatorChar))
        {
            database = Path.GetFileNameWithoutExtension(database);
            isFileDatabase = true;
        }
        else
        {
            isFileDatabase = false;
        }

        return database;
    }

}
