using System;
using System.Threading;

using GreenPipes;
using GreenPipes.Policies;

namespace Cogito.GreenPipes.Policies
{

    public class DelegateRetryContext<TContext> :
        BaseRetryContext<TContext>,
        RetryContext<TContext>
        where TContext : class, PipeContext
    {

        readonly DelegateRetryPolicy policy;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="retryCount"></param>
        /// <param name="cancellationToken"></param>
        public DelegateRetryContext(DelegateRetryPolicy policy, TContext context, Exception exception, int retryCount, CancellationToken cancellationToken) :
            base(context, exception, retryCount, cancellationToken)
        {
            this.policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        /// <summary>
        /// Returns the delay until the next execution.
        /// </summary>
        public override TimeSpan? Delay => policy.GetDelay(Exception, RetryCount);

        bool RetryContext<TContext>.CanRetry(Exception exception, out RetryContext<TContext> retryContext)
        {
            retryContext = new DelegateRetryContext<TContext>(policy, Context, Exception, RetryCount + 1, CancellationToken);
            return policy.IsHandled(exception) && policy.GetDelay(exception, RetryAttempt) != null;
        }

    }

}
