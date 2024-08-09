using Librame.Extensions.Configuration;
using Librame.Extensions.Dependency;
using System;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class LockDependencyTests
    {
        private readonly StringBuilder _builder = new();


        [Fact]
        public void LockTest()
        {
            var maxLevel = Environment.ProcessorCount / 2;
            var currentLevel = 1;

            WriteLineAtMaxLevel();

            var filePath = "lockers_test.txt".SetFileBasePath();
            File.WriteAllText(filePath.ToString(), _builder.ToString());

            void WriteLineAtMaxLevel()
            {
                DependencyRegistration.CurrentContext.Locks.Lock(index =>
                {
                    WriteLine(index);

                    if (currentLevel < maxLevel)
                    {
                        currentLevel++;
                        WriteLineAtMaxLevel();
                    }
                });
            }

            filePath.Delete();
        }

        private void WriteLine(int index)
        {
            var thread = Thread.CurrentThread;
            _builder.AppendLine($"Thread {thread.ManagedThreadId} use Locker {index}.");

            var sleep = 100;
            Thread.Sleep(sleep);
            _builder.AppendLine($"Thread {thread.ManagedThreadId} sleep {sleep} milliseconds.");

            _builder.AppendLine($"Thread {thread.ManagedThreadId} release Locker {index}.");
        }

    }
}
