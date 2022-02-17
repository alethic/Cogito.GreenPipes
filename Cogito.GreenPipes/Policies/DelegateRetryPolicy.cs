using System;

using GreenPipes;

namespace Cogito.GreenPipes.Policies
{

    /// <summary>
    /// Describes a retry policy that invoke a delegate to obtain the timespan for the delay until the next attempt.
    /// </summary>
    public class DelegateRetryPolicy :
        IRetryPolicy
    {

        readonly IExceptionFilter filter;
        readonly DelegateRetryDelayDelegate getDelay;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="getDelay"></param>
        public DelegateRetryPolicy(IExceptionFilter filter, DelegateRetryDelayDelegate getDelay)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
            this.getDelay = getDelay ?? throw new ArgumentNullException(nameof(getDelay));
        }

        public void Probe(ProbeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Set(new
            {
                Policy = "Delegate"
            });

            filter.Probe(context);
        }

        RetryPolicyContext<T> IRetryPolicy.CreatePolicyContext<T>(T context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return new DelegateRetryPolicyContext<T>(this, context);
        }

        public bool IsHandled(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return filter.Match(exception);
        }

        /// <summary>
        /// Gets the delay until the next execution, or <c>null</c> if no executions remain.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="retryAttempt"></param>
        /// <returns></returns>
        public TimeSpan? GetDelay(Exception exception, int retryAttempt)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return getDelay(exception, retryAttempt);
        }

    }

}
