#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

class InternalProcessorManager : IProcessorManager
{
    private readonly IServiceProvider _services;


    public InternalProcessorManager(IServiceProvider services)
    {
        _services = services;
    }


    public TProcessor UseProcessor<TProcessor>()
        where TProcessor : IProcessor
        => _services.GetRequiredService<TProcessor>();

    public IProcessor UseProcessor(Type processorType)
    {
        processorType.NotAssignableToBaseType<IProcessor>();
        return (IProcessor)_services.GetRequiredService(processorType);
    }

}
