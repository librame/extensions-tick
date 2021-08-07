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
using System.Collections.Generic;

namespace Librame.Extensions.Data.Access
{
    class DefaultAccessorManager : IAccessorManager
    {
        private IAccessorAggregator _aggregator;
        private IAccessorSlicer _slicer;
        private IAccessorMigrator _migrator;
        private AccessOptions _options;

        private IAccessor? _readAccessor;
        private IAccessor? _writeAccessor;


        public DefaultAccessorManager(DataExtensionBuilder builder,
            IAccessorResolver resolver, IAccessorAggregator aggregator,
            IAccessorSlicer slicer, IAccessorMigrator migrator)
        {
            _aggregator = aggregator;
            _slicer = slicer;
            _migrator = migrator;
            _options = builder.Options.Access;

            Descriptors = resolver.ResolveDescriptors();
            if (Descriptors.Count < 1)
                throw new ArgumentNullException($"The accessor descriptor not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");
        }


        public IReadOnlyList<AccessorDescriptor> Descriptors { get; init; }


        public IAccessor GetReadAccessor(Func<IAccessor, bool>? customSliceFunc = null)
        {
            OnAutomaticMigration();

            if (_readAccessor == null)
            {
                _readAccessor = _options.Relationship == AccessorsRelationship.Aggregation
                    ? _aggregator.AggregateReadAccessors(Descriptors)
                    : _slicer.SliceReadAccessors(Descriptors, customSliceFunc ?? _options.DefaultSliceFunc);
            }
            
            return _readAccessor!;
        }

        public IAccessor GetWriteAccessor(Func<IAccessor, bool>? customSliceFunc = null)
        {
            OnAutomaticMigration();

            if (_writeAccessor == null)
            {
                _writeAccessor = _options.Relationship == AccessorsRelationship.Aggregation
                    ? _aggregator.AggregateWriteAccessors(Descriptors)
                    : _slicer.SliceWriteAccessors(Descriptors, customSliceFunc ?? _options.DefaultSliceFunc);
            }

            return _writeAccessor!;
        }


        private void OnAutomaticMigration()
        {
            if (_options.AutomaticMigration)
                _migrator.Migrate(Descriptors);
        }

    }
}
