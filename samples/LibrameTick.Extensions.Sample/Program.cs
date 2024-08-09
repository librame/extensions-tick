// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using LibrameTick.Extensions.Sample;

BenchmarkRunner.Run<BinarySerializerBenchmark>();
