#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Network;

class InternalHttpClientInvokerFactory : IHttpClientInvokerFactory
{
    private readonly IHttpClientFactory _clientFactory;


    public InternalHttpClientInvokerFactory(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public HttpClient CreateClient(HttpClientInvokingOptions options)
    {
        return string.IsNullOrEmpty(options.ClientName)
            ? HttpClientInvoker.InvokeClient(options)
            : _clientFactory.CreateClient(options.ClientName);
    }

}
