using System;
using System.Threading.Tasks;

using GreenPipes;

namespace Cogito.GreenPipes
{

    /// <summary>
    /// Pauses requests until the lock returned by the lock manager allows them to pass.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class SynchronizationFilter<TContext> : IFilter<TContext>
        where TContext : class, PipeContext
    {

        readonly Func<TContext, Task<IAsyncDisposable>> acquire;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="acquire"></param>
        public SynchronizationFilter(Func<TContext, Task<IAsyncDisposable>> acquire)
        {
            this.acquire = acquire ?? throw new ArgumentNullException(nameof(acquire));
        }

        public void Probe(ProbeContext context)
        {

        }

        public async Task Send(TContext context, IPipe<TContext> next)
        {
            // acquire lock
            var lck = await acquire(context);

            try
            {
                // continue onward
                await next.Send(context);
            }
            finally
            {
                if (lck != null)
                {
                    // release lock
                    await lck.DisposeAsync();
                }
            }
        }

    }

}
