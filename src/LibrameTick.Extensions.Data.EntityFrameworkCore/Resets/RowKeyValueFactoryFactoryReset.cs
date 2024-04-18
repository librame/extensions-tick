using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Resets
{
    public class RowKeyValueFactoryFactoryReset : IRowKeyValueFactoryFactory
    {
        public virtual IRowKeyValueFactory Create(IUniqueConstraint key)
        => key.Columns.Count == 1
            ? (IRowKeyValueFactory)_createMethod
                .MakeGenericMethod(key.Columns.First().ProviderClrType)
                .Invoke(null, new object[] { key })!
            : new CompositeRowKeyValueFactory(key);

        private static readonly MethodInfo _createMethod = typeof(RowKeyValueFactoryFactoryReset).GetTypeInfo()
            .GetDeclaredMethod(nameof(CreateSimpleFactory))!;

        //[UsedImplicitly]
        private static IRowKeyValueFactory<TKey> CreateSimpleFactory<TKey>(IUniqueConstraint key)
            => new SimpleRowKeyValueFactoryReset<TKey>(key);
    }
}
