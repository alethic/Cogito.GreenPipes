using System;

using Cogito.GreenPipes.Policies;

using GreenPipes.Configurators;

namespace Cogito.GreenPipes
{

    public static class RetryExtensions
    {

        /// <summary>
        /// Configures the retry policy to consult a delegate in determining the delay.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="getDelay"></param>
        /// <returns></returns>
        public static IRetryConfigurator Delegate(this IRetryConfigurator configurator, DelegateRetryDelayDelegate getDelay)
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));
            if (getDelay == null)
                throw new ArgumentNullException(nameof(getDelay));

            configurator.SetRetryPolicy(filter => new DelegateRetryPolicy(filter, getDelay));
            return configurator;
        }

    }

}
