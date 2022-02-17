using System;
using System.Threading.Tasks;

using GreenPipes;

namespace Cogito.GreenPipes
{

    public static class SynchronizationExtensions
    {

        /// <summary>
        /// Adds a filter to the pipe that acquires a lock upon entry and releases it upon exit.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="acquire"></param>
        public static void UseLock<TContext>(
            this IPipeConfigurator<TContext> configurator,
            Func<TContext, Task<IAsyncDisposable>> acquire)
            where TContext : class, PipeContext
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));
            if (acquire == null)
                throw new ArgumentNullException(nameof(acquire));

            configurator.AddPipeSpecification(new SynchronizationFilterSpecification<TContext>(acquire));
        }

    }

}
