using System;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    /// <summary>
    /// Certaines implémentations de Dispose sont foireuses et laissent passer des exceptions. Elles sont mangées ici
    /// </summary>
    /// <typeparam name="TDisposable"></typeparam>
    public static class RectifyDispose<TDisposable> where TDisposable : IDisposable
    {
        public static TReturn UseDisposableResource<TReturn>(TDisposable resource, Func<TDisposable, TReturn> usage)
        {
            try
            {
                return usage(resource);
            }
            finally
            {
                try
                {

                    resource.Dispose();
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static void UseDisposableResource(TDisposable resource, Action<TDisposable> usage)
        {
            try
            {
                usage(resource);
            }
            finally
            {
                try
                {
                    resource.Dispose();
                }
                catch
                {
                    // ignore
                }
            }
        }
    }

    /// <summary>
    /// Certaines implémentations de Dispose sont foireuses et laissent passer des exceptions. Elles sont mangées ici
    /// </summary>
    /// <typeparam name="TAsyncDisposable"></typeparam>
    public static class AsyncRectifyDispose<TAsyncDisposable> where TAsyncDisposable : IAsyncDisposable
    {
        public static async Task<TReturn> UseDisposableResource<TReturn>(TAsyncDisposable resource, Func<TAsyncDisposable, Task<TReturn>> usage)
        {
            try
            {
                return await usage(resource);
            }
            finally
            {
                try
                {

                    await resource.DisposeAsync();
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static async Task UseDisposableResource(TAsyncDisposable resource, Func<TAsyncDisposable, Task> usage)
        {
            try
            {
                await usage(resource);
            }
            finally
            {
                try
                {
                    await resource.DisposeAsync();
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}
