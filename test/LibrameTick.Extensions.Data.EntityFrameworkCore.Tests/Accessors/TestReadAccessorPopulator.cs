using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    class TestReadAccessorPopulator : AbstractAccessorPopulator<TestReadAccessor>
    {
        public TestReadAccessorPopulator(TestReadAccessor accessor)
            : base(accessor)
        {
        }


        protected override int PopulateCore(IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        protected override Task<int> PopulateCoreAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
