using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cogito.GreenPipes
{

    /// <summary>
    /// Represents a simple lock that can be released.
    /// </summary>
    public class DelegateLock : IAsyncDisposable
    {

        readonly Func<CancellationToken, Task> release;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="release"></param>
        public DelegateLock(Func<CancellationToken, Task> release)
        {
            this.release = release ?? throw new ArgumentNullException(nameof(release));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="release"></param>
        public DelegateLock(Func<Task> release)
        {
            this.release = (CancellationToken cancellationToken) => release();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="release"></param>
        public DelegateLock(Action release)
        {
            this.release = (CancellationToken cancellationToken) => { release(); return Task.CompletedTask; };
        }

        /// <summary>
        /// Releases the lock.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await release(CancellationToken.None);
        }

    }

}
