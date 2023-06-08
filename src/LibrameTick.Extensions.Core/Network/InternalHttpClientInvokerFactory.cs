#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Microparts;

namespace Librame.Extensions.Network;

sealed internal class InternalHttpClientInvokerFactory : IHttpClientInvokerFactory
{
    private readonly IHttpClientFactory _clientFactory;


    public InternalHttpClientInvokerFactory(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public HttpClient CreateClient(HttpClientOptions options)
    {
        return string.IsNullOrEmpty(options.ClientName)
            ? MicropartActivator.CreateHttpClient(options).Unwrap()
            : _clientFactory.CreateClient(options.ClientName);
    }

}
