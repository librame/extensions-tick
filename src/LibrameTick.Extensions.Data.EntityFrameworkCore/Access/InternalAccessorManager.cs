#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Access
{
    class InternalAccessorManager : IAccessorManager
    {
        private IAccessorAggregator _aggregator;
        //private IAccessorSlicer _slicer;
        private IAccessorMigrator _migrator;
        private AccessOptions _options;

        private IAccessor? _readAccessor;
        private IAccessor? _writeAccessor;

        private readonly Dictionary<int, IAccessor?> _readGroupAccessors
            = new Dictionary<int, IAccessor?>();

        private readonly Dictionary<int, IAccessor?> _writeGroupAccessors
            = new Dictionary<int, IAccessor?>();


        public InternalAccessorManager(DataExtensionBuilder builder,
            IAccessorResolver resolver, IAccessorAggregator aggregator,
            IAccessorMigrator migrator)
        {
            _aggregator = aggregator;
            _migrator = migrator;
            _options = builder.Options.Access;

            Descriptors = resolver.ResolveDescriptors();
            if (Descriptors.Count < 1)
                throw new ArgumentNullException($"The accessor descriptor not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");

            InitializeAccessors();
        }


        public IReadOnlyList<AccessorDescriptor> Descriptors { get; init; }


        private void InitializeAccessors()
        {
            if (Descriptors.Count == 1)
            {
                _readAccessor = _writeAccessor = Descriptors[0].Accessor;
            }
            else
            {
                foreach (var group in Descriptors.GroupBy(descr => descr.Group))
                {
                    var groupList = group.ToList();

                    _readGroupAccessors.Add(group.Key, _aggregator.AggregateReadAccessors(groupList));
                    _writeGroupAccessors.Add(group.Key, _aggregator.AggregateWriteAccessors(groupList));
                }

                _readAccessor = _readGroupAccessors.FirstOrDefault().Value ?? Descriptors[0].Accessor;
                _writeAccessor = _writeGroupAccessors.FirstOrDefault().Value ?? Descriptors[0].Accessor;
            }
        }

        private void OnAutomaticMigration()
        {
            if (_options.AutomaticMigration)
                _migrator.Migrate(Descriptors);
        }


        public IAccessor GetReadAccessor(int? group = null)
        {
            OnAutomaticMigration();

            if (group.HasValue && _readGroupAccessors.TryGetValue(group.Value, out var accessor))
                return accessor ?? _readAccessor!;

            return _readAccessor!;
        }

        public IAccessor GetWriteAccessor(int? group = null)
        {
            OnAutomaticMigration();

            if (group.HasValue && _writeGroupAccessors.TryGetValue(group.Value, out var accessor))
                return accessor ?? _writeAccessor!;

            return _writeAccessor!;
        }

    }
}
