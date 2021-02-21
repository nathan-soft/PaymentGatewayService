using PaymentProcessor.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentProcessor.Code
{
    public static class AppHelper
    {
        /// <summary>
        /// Tries to execute an action for the number of times specified before eventually failing.
        /// </summary>
        /// <param name="action">The action to retry or execute.</param>
        /// <param name="retryInterval">Number of time in milliseconds to wait before trying to re-execute the action. </param>
        /// <param name="retryCount">Number of time to try and perform the action.</param>
        /// <returns>A task that resolves to nothing/void once completed. throws Exception on error.</returns>
        public static async Task RetryAsync(Action action, TimeSpan retryInterval, int retryCount)
        {
            var exceptions = new List<Exception>();

            //exit out of the loop once "retry" equals "retryCount"
            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    // Execute method
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    //log error
                    //add exception to the list.
                    exceptions.Add(ex);

                    //gotten to the last loop
                    if ((retry + 1) == retryCount)
                    {
                        //for cases when retry method should only run once
                        //or retry count equals 1
                        if (retry < 1)
                            throw;
                        else
                            throw new AggregateException(exceptions);
                        
                    }

                    if (retry > 1)
                    {
                        //operation has been retried atleast twice
                        //maybe consider increasing the delay time
                        retryInterval.Multiply(2);
                    }

                    // Wait to retry the operation.
                    await Task.Delay(retryInterval);
                }
            }
        }
    }
}
