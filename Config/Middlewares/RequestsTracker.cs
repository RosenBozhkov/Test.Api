using System;
using System.Threading;
using inacs.v8.nuget.DevAttributes;

namespace Config.Middlewares;

/// <summary>
/// Tracker for requests and responses count.
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public class RequestsTracker
{
    private long _ongoingRequestsCount;
    private bool _shouldRefuseRequests;

    /// <summary>
    /// Property showing if the service should accept new requests
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public bool ShouldRefuseRequests
    {
        get => _shouldRefuseRequests;
        set
        {
            //Fix exception type
            if (_shouldRefuseRequests) throw new InvalidOperationException();

            _shouldRefuseRequests = value;
        }
    }

    /// <summary>
    /// Property for the currently running requests
    /// </summary>
    public long OngoingRequestsCount => Interlocked.Read(ref _ongoingRequestsCount);
    /// <summary>
    /// Increases the request count
    /// </summary>
    public void Increment() => Interlocked.Increment(ref _ongoingRequestsCount);
    /// <summary>
    /// Decreases the request count
    /// </summary>
    public void Decrement() => Interlocked.Decrement(ref _ongoingRequestsCount);
}