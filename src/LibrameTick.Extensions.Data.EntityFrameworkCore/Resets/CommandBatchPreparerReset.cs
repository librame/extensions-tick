using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Librame.Extensions.Data.Resets
{
    public class CommandBatchPreparerReset : CommandBatchPreparer
    {
        public CommandBatchPreparerReset(CommandBatchPreparerDependencies dependencies)
            : base(dependencies)
        {
        }


        public override IEnumerable<ModificationCommandBatch> BatchCommands(IList<IUpdateEntry> entries, IUpdateAdapter updateAdapter)
        {
            return base.BatchCommands(entries, updateAdapter);
        }

    }
}
