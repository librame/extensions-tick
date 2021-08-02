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

namespace Librame.Extensions.Data.Accessors
{
    class AccessorManager : IAccessorManager
    {
        private IAccessorAggregator _aggregator;
        private IAccessorSlicer _slicer;
        private AccessorsRelationship _managementPattern;
        private IReadOnlyList<AccessorDescriptor> _descriptors;

        private IAccessor? _readAccessor;
        private IAccessor? _writeAccessor;


        public AccessorManager(DataExtensionBuilder builder, IAccessorResolver resolver,
            IAccessorAggregator aggregator, IAccessorSlicer slicer)
        {
            builder.NotNull(nameof(builder));
            resolver.NotNull(nameof(resolver));

            _aggregator = aggregator.NotNull(nameof(aggregator));
            _slicer = slicer.NotNull(nameof(slicer));

            _descriptors = resolver.ResolveDescriptors();
            if (_descriptors.Count < 1)
                throw new ArgumentNullException("The accessor descriptor not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");

            _managementPattern = builder.Options.Access.Relationship;
            CustomSliceFunc = builder.Options.Access.DefaultSliceFunc;
        }


        public Func<IAccessor, bool>? CustomSliceFunc { get; set; }


        public IAccessor GetReadAccessor()
        {
            if (_readAccessor == null)
            {
                _readAccessor = _managementPattern == AccessorsRelationship.Aggregation
                    ? _aggregator.AggregateReadAccessors(_descriptors)
                    : _slicer.SliceReadAccessors(_descriptors, CustomSliceFunc);
            }
            
            return _readAccessor!;
        }

        public IAccessor GetWriteAccessor()
        {
            if (_writeAccessor == null)
            {
                _writeAccessor = _managementPattern == AccessorsRelationship.Aggregation
                    ? _aggregator.AggregateWriteAccessors(_descriptors)
                    : _slicer.SliceWriteAccessors(_descriptors, CustomSliceFunc);
            }

            return _writeAccessor!;
        }

    }
}
