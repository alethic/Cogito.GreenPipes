using System;
using System.Threading;

using GreenPipes;
using GreenPipes.Policies;

namespace Cogito.GreenPipes.Policies
{

    public class DelegateRetryPolicyContext<TContext> :
        BaseRetryPolicyContext<TContext>
        where TContext : class, PipeContext
    {

        readonly DelegateRetryPolicy policy;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="context"></param>
        public DelegateRetryPolicyContext(DelegateRetryPolicy policy, TContext context) :
            base(policy, context)
        {
            this.policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        protected override RetryContext<TContext> CreateRetryContext(Exception exception, CancellationToken cancellationToken)
        {
            return new DelegateRetryContext<TContext>(policy, Context, exception, 0, cancellationToken);
        }

    }

}