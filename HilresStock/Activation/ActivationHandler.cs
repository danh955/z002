// <copyright file="ActivationHandler.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Activation
{
    using System.Threading.Tasks;

    /// <summary>
    /// Activation handler class.
    /// For more information on understanding and extending activation flow see
    /// https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/activation.md.
    /// </summary>
    internal abstract class ActivationHandler
    {
        /// <summary>
        /// Can handle.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>True if can handle.</returns>
        public abstract bool CanHandle(object args);

        /// <summary>
        /// Handle async.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Task.</returns>
        public abstract Task HandleAsync(object args);
    }

    /// <summary>
    /// Activation handler.
    /// Extend this class to implement new ActivationHandlers.
    /// </summary>
    /// <typeparam name="T">ActivationHandler.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type

    internal abstract class ActivationHandler<T> : ActivationHandler
#pragma warning restore SA1402 // File may only contain a single type
        where T : class
    {
        /// <inheritdoc/>
        public override async Task HandleAsync(object args)
        {
            await this.HandleInternalAsync(args as T).ConfigureAwait(true);
        }

        /// <inheritdoc/>
        public override bool CanHandle(object args)
        {
            // CanHandle checks the args is of type you have configured
            return args is T && this.CanHandleInternal(args as T);
        }

        /// <summary>
        /// Handle Internal Async.
        /// Override this method to add the activation logic in your activation handler.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Task.</returns>
        protected abstract Task HandleInternalAsync(T args);

        /// <summary>
        /// Can Handle Internal.
        /// You can override this method to add extra validation on activation args
        /// to determine if your ActivationHandler should handle this activation args.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>True if it can handle internal.</returns>
        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }
    }
}
