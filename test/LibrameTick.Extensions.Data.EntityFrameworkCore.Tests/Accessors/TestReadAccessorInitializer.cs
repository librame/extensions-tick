using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    class TestReadAccessorInitializer : AbstractAccessorInitializer<TestReadAccessor>
    {
        public TestReadAccessorInitializer(TestReadAccessor accessor)
            : base(accessor)
        {
        }


        protected override void Populate(IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        protected override Task PopulateAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
