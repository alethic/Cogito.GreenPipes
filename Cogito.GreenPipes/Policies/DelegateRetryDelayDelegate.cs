using System;

namespace Cogito.GreenPipes.Policies
{

    /// <summary>
    /// Returns the delay until the next attempt.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="retryAttempt"></param>
    /// <returns></returns>
    public delegate TimeSpan? DelegateRetryDelayDelegate(Exception exception, int retryAttempt);

}
