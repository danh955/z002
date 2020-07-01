// <copyright file="ResourceExtensions.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Helpers
{
    using Windows.ApplicationModel.Resources;

    /// <summary>
    /// Resource extensions class.
    /// </summary>
    internal static class ResourceExtensions
    {
        private static readonly ResourceLoader ResLoader = new ResourceLoader();

        /// <summary>
        /// Get Localized.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <returns>String.</returns>
        public static string GetLocalized(this string resourceKey)
        {
            return ResLoader.GetString(resourceKey);
        }
    }
}
