using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GreenPipes;

namespace Cogito.GreenPipes
{

    public class SynchronizationFilterSpecification<TContext> : IPipeSpecification<TContext>
        where TContext : class, PipeContext
    {

        readonly Func<TContext, Task<IAsyncDisposable>> acquire;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="acquire"></param>
        public SynchronizationFilterSpecification(Func<TContext, Task<IAsyncDisposable>> acquire)
        {
            this.acquire = acquire ?? throw new ArgumentNullException(nameof(acquire));
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new SynchronizationFilter<TContext>(acquire));
        }

        public IEnumerable<ValidationResult> Validate()
        {
            yield break;
        }

    }

}
