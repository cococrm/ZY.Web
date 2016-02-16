using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace ZY.Core
{
    public static class AsyncHelper
    {

        public static TResult RunAsync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        public static void RunAsync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
