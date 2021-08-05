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
using System.Linq;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义 MonggoDB 标识参数。
    /// </summary>
    public struct MongoIdentificationParameters : IEquatable<MongoIdentificationParameters>
    {
        private byte[] _timestampBytes;


        /// <summary>
        /// 使用字节数组构造一个 <see cref="MongoIdentificationParameters"/>。
        /// </summary>
        /// <param name="timestampBytes">给定的时间戳字节数组。</param>
        public MongoIdentificationParameters(byte[] timestampBytes)
        {
            if (timestampBytes.Length != 12)
                throw new ArgumentOutOfRangeException(nameof(timestampBytes), "value should be 12 characters");

            var copyIndex = 0;

            var buffer = new byte[4];
            Array.Copy(timestampBytes, copyIndex, buffer, 0, 4);
            Array.Reverse(buffer);

            Timestamp = BitConverter.ToInt32(buffer, 0);

            copyIndex += 4;
            var machineIdBytes = new byte[4];
            Array.Copy(timestampBytes, copyIndex, machineIdBytes, 0, 3);

            MachineId = BitConverter.ToInt32(machineIdBytes, 0);

            copyIndex += 3;
            var processIdBytes = new byte[4];
            Array.Copy(timestampBytes, copyIndex, processIdBytes, 0, 2);
            Array.Reverse(processIdBytes);

            ProcessId = BitConverter.ToInt32(processIdBytes, 0);

            copyIndex += 2;
            var incrementBytes = new byte[4];
            Array.Copy(timestampBytes, copyIndex, incrementBytes, 0, 3);
            Array.Reverse(incrementBytes);

            Increment = BitConverter.ToInt32(incrementBytes, 0);

            _timestampBytes = timestampBytes;
        }


        /// <summary>
        /// 增量。
        /// </summary>
        public int Increment { get; private set; }

        /// <summary>
        /// 主机标识。
        /// </summary>
        public int MachineId { get; private set; }

        /// <summary>
        /// 进程标识。
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// 时间戳。
        /// </summary>
        public int Timestamp { get; private set; }


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="MongoIdentificationParameters"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(MongoIdentificationParameters other)
            => _timestampBytes.SequenceEqual(other._timestampBytes);


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => _timestampBytes.AsHexString();


        /// <summary>
        /// 解析标识字符串。
        /// </summary>
        /// <param name="id">给定的标识字符串（如：“0000-0000-0000-0000-0000-0000”）。</param>
        /// <returns>返回 <see cref="MongoIdentificationParameters"/>。</returns>
        public static MongoIdentificationParameters Parse(string id)
            => new MongoIdentificationParameters(id.FromHexString());

    }
}
