// <copyright file="SettingsStorageExtensions.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace HilresStock.Helpers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using HilresStock.Core.Helpers;
    using Windows.Storage;
    using Windows.Storage.Streams;

    /// <summary>
    /// Settings Storage Extensions class.
    /// Use these extension methods to store and retrieve local and roaming app data
    /// More details regarding storing and retrieving app data at https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data.
    /// </summary>
    public static class SettingsStorageExtensions
    {
        private const string FileExtension = ".json";

        /// <summary>
        /// Is Roaming Storage Available.
        /// </summary>
        /// <param name="appData">Application data.</param>
        /// <returns>True if Roaming Storage Available.</returns>
        public static bool IsRoamingStorageAvailable(this ApplicationData appData)
        {
            if (appData == null)
            {
                throw new ArgumentNullException(nameof(appData));
            }

            return appData.RoamingStorageQuota == 0;
        }

        /// <summary>
        /// Save Async.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="folder">Storage Folder.</param>
        /// <param name="name">File name.</param>
        /// <param name="content">Content.</param>
        /// <returns>Task.</returns>
        public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
            var fileContent = await Json.StringifyAsync(content).ConfigureAwait(false);

            await FileIO.WriteTextAsync(file, fileContent);
        }

        /// <summary>
        /// Read Async.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="folder">Storage Folder.</param>
        /// <param name="name">File name.</param>
        /// <returns>Item.</returns>
        public static async Task<T> ReadAsync<T>(this StorageFolder folder, string name)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
            {
                return default;
            }

            var file = await folder.GetFileAsync($"{name}.json");
            var fileContent = await FileIO.ReadTextAsync(file);

            return await Json.ToObjectAsync<T>(fileContent).ConfigureAwait(false);
        }

        /// <summary>
        /// Save Async.
        /// </summary>
        /// <typeparam name="T">Data Type.</typeparam>
        /// <param name="settings">ApplicationDataContainer.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Data value.</param>
        /// <returns>Task.</returns>
        public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.SaveString(key, await Json.StringifyAsync(value).ConfigureAwait(false));
        }

        /// <summary>
        /// Save String.
        /// </summary>
        /// <param name="settings">Application Data Container.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SaveString(this ApplicationDataContainer settings, string key, string value)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Values[key] = value;
        }

        /// <summary>
        /// Read Async.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="settings">Application Data Container.</param>
        /// <param name="key">Key.</param>
        /// <returns>Item.  Default if not found.</returns>
        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.Values.TryGetValue(key, out object obj))
            {
                return await Json.ToObjectAsync<T>((string)obj).ConfigureAwait(false);
            }

            return default;
        }

        /// <summary>
        /// Save File Async.
        /// </summary>
        /// <param name="folder">Storage folder.</param>
        /// <param name="content">Byte array.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="options">Creation Collision Option.</param>
        /// <returns>StorageFile.</returns>
        public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("ExceptionSettingsStorageExtensionsFileNameIsNullOrEmpty".GetLocalized(), nameof(fileName));
            }

            var storageFile = await folder.CreateFileAsync(fileName, options);
            await FileIO.WriteBytesAsync(storageFile, content);
            return storageFile;
        }

        /// <summary>
        /// Read File Async.
        /// </summary>
        /// <param name="folder">Storage folder.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Byte array.</returns>
        public static async Task<byte[]> ReadFileAsync(this StorageFolder folder, string fileName)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

            if ((item != null) && item.IsOfType(StorageItemTypes.File))
            {
                var storageFile = await folder.GetFileAsync(fileName);
                byte[] content = await storageFile.ReadBytesAsync().ConfigureAwait(false);
                return content;
            }

            return null;
        }

        /// <summary>
        /// Read Bytes Async.
        /// </summary>
        /// <param name="file">Storage File.</param>
        /// <returns>Byte array.</returns>
        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
        {
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    using (var reader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        var bytes = new byte[stream.Size];
                        reader.ReadBytes(bytes);
                        return bytes;
                    }
                }
            }

            return null;
        }

        private static string GetFileName(string name)
        {
            return string.Concat(name, FileExtension);
        }
    }
}
