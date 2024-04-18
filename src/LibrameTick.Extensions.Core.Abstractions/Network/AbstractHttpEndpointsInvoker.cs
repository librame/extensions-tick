#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependencies;

namespace Librame.Extensions.Network;

/// <summary>
/// 抽象实现 <see cref="IHttpEndpointsInvoker"/> 的 HTTP 端点集合调用器。
/// </summary>
public abstract class AbstractHttpEndpointsInvoker : IHttpEndpointsInvoker
{
    private readonly ILogger _logger;
    private readonly IHttpClientInvokerFactory _factory;

    private HttpClientRequestOptions _requestOptions;
    private AlgorithmOptions _algorithmOptions;


    /// <summary>
    /// 构造一个 <see cref="AbstractHttpEndpointsInvoker"/>。
    /// </summary>
    /// <param name="logger">给定的 <see cref="ILogger"/>。</param>
    /// <param name="factory">给定的 <see cref="IHttpClientInvokerFactory"/>。</param>
    /// <param name="requestOptions">给定的 <see cref="HttpClientRequestOptions"/>。</param>
    /// <param name="algorithmOptions">给定的 <see cref="AlgorithmOptions"/>。</param>
    protected AbstractHttpEndpointsInvoker(ILogger logger, IHttpClientInvokerFactory factory,
        HttpClientRequestOptions requestOptions, AlgorithmOptions algorithmOptions)
    {
        _logger = logger;
        _factory = factory;
        _requestOptions = requestOptions;
        _algorithmOptions = algorithmOptions;
    }


    /// <summary>
    /// 字符编码。
    /// </summary>
    public Encoding Encoding
        => _algorithmOptions.Encoding;


    /// <summary>
    /// 异步调用端点集合。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含结果集合的异步操作。</returns>
    public virtual async Task<IEnumerable<string>?> InvokeAsync(CancellationToken cancellationToken = default)
    {
        if (_requestOptions.Endpoints.Count < 1)
            return null;

        var results = new List<string>();

        Uri? lastUri = null;
        var client = _factory.CreateClient(_requestOptions.HttpClient);

        _logger.LogInformation($"Use text encoding: {Encoding.AsEncodingName()}");

        foreach (var endpoint in _requestOptions.Endpoints)
        {
            if (lastUri is not null)
            {
                if (client.DefaultRequestHeaders.Referrer is null)
                {
                    client.DefaultRequestHeaders.Referrer = lastUri;
                    _logger.LogInformation($"Set default request header referrer: {lastUri}");
                }

                if (string.IsNullOrEmpty(client.DefaultRequestHeaders.Host))
                {
                    client.DefaultRequestHeaders.Host = lastUri.Host;
                    _logger.LogInformation($"Set default request header host: {lastUri.Host}");
                }
            }

            var currentUri = endpoint.ToUri();

            try
            {
                var response = await GetHttpResponseMessageAsync(client, currentUri, endpoint, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var content = response.ReasonPhrase ?? response.StatusCode.ToString();
                    results.Add(content);

                    _logger.LogWarning(content);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    results.Add(content);

                    _logger.LogInformation(content);

                    endpoint.ResponsedAction?.Invoke(content);
                }

                if (!string.IsNullOrEmpty(endpoint.ResponseCookieName)
                    && response.Headers.TryGetValues(endpoint.ResponseCookieName, out var values))
                {
                    var cookieValues = values.ToArray();
                    _requestOptions.VerifyId = cookieValues[endpoint.ResponseCookieIndex];

                    _logger.LogInformation($"Set {nameof(_requestOptions.VerifyId)}: {_requestOptions.VerifyId}");
                }

                lastUri = currentUri;
            }
            catch (Exception ex)
            {
                var message = ex.GetInnermostMessage();

                results.Add(message);
                _logger.LogError(message);
            }
        }

        return results;
    }

    /// <summary>
    /// 异步获取 HTTP 响应消息。
    /// </summary>
    /// <param name="client">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="uri">给定的 <see cref="Uri"/> 请求。</param>
    /// <param name="endpointOptions">给定的 <see cref="HttpEndpointOptions"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回包含 <see cref="HttpRequestMessage"/> 的异步操作。</returns>
    protected virtual Task<HttpResponseMessage> GetHttpResponseMessageAsync(HttpClient client, Uri uri,
        HttpEndpointOptions endpointOptions, CancellationToken cancellationToken)
    {
        Task<HttpResponseMessage> response;

        switch (endpointOptions.Method)
        {
            case HttpClientMethods.Get:
                {
                    _logger.LogInformation($"Starting get the endpoint request: {uri}");
                    response = client.GetAsync(uri, cancellationToken);
                }
                break;

            case HttpClientMethods.Delete:
                {
                    _logger.LogInformation($"Starting delete the endpoint request: {uri}");
                    response = client.DeleteAsync(uri, cancellationToken);
                }
                break;

            case HttpClientMethods.Post:
                {
                    _logger.LogInformation($"Starting post the endpoint request: {uri}");
                    response = client.PostAsync(uri, GetHttpContent(endpointOptions), cancellationToken);
                }
                break;

            case HttpClientMethods.Put:
                {
                    _logger.LogInformation($"Starting post the endpoint request: {uri}");
                    response = client.PutAsync(uri, GetHttpContent(endpointOptions), cancellationToken);
                }
                break;

            case HttpClientMethods.Patch:
                {
                    _logger.LogInformation($"Starting post the endpoint request: {uri}");
                    response = client.PatchAsync(uri, GetHttpContent(endpointOptions), cancellationToken);
                }
                break;

            default:
                goto case HttpClientMethods.Get;
        }

        if (!string.IsNullOrEmpty(endpointOptions.WaitTimeout))
        {
            _logger.LogInformation($"Set wait timeout: {endpointOptions.WaitTimeout}");
            response = response.WaitAsync(TimeSpan.Parse(endpointOptions.WaitTimeout), cancellationToken);
        }

        return response;
    }

    /// <summary>
    /// 获取 HTTP 内容。
    /// </summary>
    /// <param name="endpointOptions">给定的 <see cref="HttpEndpointOptions"/>。</param>
    /// <returns>返回 <see cref="HttpContent"/>。</returns>
    protected virtual HttpContent? GetHttpContent(HttpEndpointOptions endpointOptions)
    {
        var parameters = endpointOptions.GetParameters();
        if (parameters is null)
            return null;

        switch (endpointOptions.ContentType)
        {
            case HttpClientContentTypes.FormUrlEncoded:
                {
                    _logger.LogInformation($"Use 'application/x-www-form-urlencoded' content type: {endpointOptions.Parameters}");
                    return new FormUrlEncodedContent(parameters);
                }

            case HttpClientContentTypes.MultipartFormData:
                {
                    _logger.LogInformation($"Use 'multipart/form-data' content type: {endpointOptions.Parameters}");

                    var content = new MultipartFormDataContent();

                    foreach (var pair in parameters)
                    {
                        if (File.Exists(pair.Value))
                        {
                            _logger.LogInformation($"Add file: {pair.Key}={pair.Value}");
                            content.Add(new StreamContent(File.Open(pair.Value, FileMode.Open)), pair.Key, pair.Value);
                        }
                        else
                        {
                            _logger.LogInformation($"Add name: {pair.Key}={pair.Value}");
                            content.Add(new StringContent(pair.Value, Encoding), pair.Key);
                        }
                    }

                    return content;
                }

            case HttpClientContentTypes.Json:
                {
                    _logger.LogInformation($"Use 'application/json' content type: {endpointOptions.Parameters}");

                    var json = JsonSerializer.Serialize(parameters);
                    return new StringContent(json, Encoding);
                }

            case HttpClientContentTypes.Stream:
                {
                    _logger.LogInformation($"Use 'binary' content type: {endpointOptions.Parameters}");
                    return new ByteArrayContent(endpointOptions.Parameters!.FromBase64String());
                }

            default:
                goto case HttpClientContentTypes.Json;
        }
    }
}
