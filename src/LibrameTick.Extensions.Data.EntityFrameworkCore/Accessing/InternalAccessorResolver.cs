#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Data.Accessing
{
    class InternalAccessorResolver : IAccessorResolver
    {
        private static readonly Type _accessorType
            = typeof(IAccessor);
        private static readonly Type _dbContextType
            = typeof(DbContext);
        private static readonly Type _dbContextOptionsTypeDefinition
            = typeof(DbContextOptions<>);

        // 限定访问器必需实现 DbContext 和 IAccessor
        private static readonly Func<ServiceDescriptor, bool> _serviceDescriptorPredicate
            = sd => sd.ServiceType.IsAssignableTo(_dbContextType) && sd.ServiceType.IsAssignableTo(_accessorType);

        private readonly DataExtensionBuilder _builder;
        private readonly IServiceProvider _provider;


        public InternalAccessorResolver(DataExtensionBuilder builder, IServiceProvider provider)
        {
            _builder = builder;
            _provider = provider;
        }


        public IReadOnlyList<AccessorDescriptor> ResolveDescriptors()
        {
            var descriptors = new List<AccessorDescriptor>();

            foreach (var options in GetAllContextOptions())
            {
                var extension = options.FindExtension<AccessorDbContextOptionsExtension>();
                if (extension != null)
                {
                    var accessor = GetAccessor(extension.ServiceType!);

                    // 默认使用访问器定义的优先级属性值
                    var priority = extension.Priority < 0 ? accessor.Priority : extension.Priority;
                    var algorithms = extension.Algorithms ?? _builder.Options.CoreOptions.Algorithms;
                    var encoding = extension.Encoding ?? _builder.Options.CoreOptions.Encoding;

                    descriptors.Add(new AccessorDescriptor(extension.ServiceType!, accessor, extension.Group,
                        extension.Interaction, extension.IsPooled, priority, algorithms, encoding));
                }
            }

            return descriptors;
        }


        private IEnumerable<DbContextOptions> GetAllContextOptions()
        {
            var serviceDescriptors = _builder.Services.Where(_serviceDescriptorPredicate);

            return serviceDescriptors.Select(GetContextOptions);
        }

        private DbContextOptions GetContextOptions(ServiceDescriptor descriptor)
            => (DbContextOptions)_provider.GetRequiredService(_dbContextOptionsTypeDefinition.MakeGenericType(descriptor.ServiceType));

        private IAccessor GetAccessor(Type serviceType)
            => (IAccessor)_provider.GetRequiredService(serviceType);

    }
}
