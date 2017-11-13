﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Threading;
using WebhookCacheInvalidationMvc.Helpers;
using WebhookCacheInvalidationMvc.Models;

namespace WebhookCacheInvalidationMvc.Services
{
    public class CacheManager : ICacheManager
    {
        #region "Fields"

        private bool _disposed = false;
        private readonly IMemoryCache _memoryCache;

        #endregion

        #region "Properties"

        public int CacheExpirySeconds
        {
            get;
            set;
        }

        #endregion

        #region "Constructors"

        public CacheManager(IOptions<ProjectOptions> projectOptions, IMemoryCache memoryCache)
        {
            CacheExpirySeconds = projectOptions.Value.CacheTimeoutSeconds;
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion

        #region "Public methods"

        public async Task<T> GetOrCreateAsync<T>(IEnumerable<string> identifierTokens, Func<Task<T>> valueFactory, Func<T, IEnumerable<IdentifierSet>> dependencyListFactory)
        {
            // Check existence of the cache entry.
            if (!_memoryCache.TryGetValue(StringHelpers.Join(identifierTokens), out T entry))
            {
                // If it doesn't exist, get it via valueFactory.
                T response = await valueFactory();

                // Create it. (Could be off-loaded to a background thread.)
                CreateEntry(identifierTokens, response, dependencyListFactory);

                return response;
            }

            return entry;
        }

        public void CreateEntry<T>(IEnumerable<string> identifierTokens, T value, Func<T, IEnumerable<IdentifierSet>> dependencyListFactory)
        {
            var dependencies = dependencyListFactory(value);

            // Restart entries' expiration period each time they're requested.
            var entryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(CacheExpirySeconds));

            // Dummy entries never expire.
            var dummyOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);

            foreach (var dependency in dependencies)
            {
                var dummyIdentifierTokens = new List<string> { "dummy", dependency.Type, dependency.Codename };
                var dummyKey = StringHelpers.Join(dummyIdentifierTokens);

                // Dummy entries hold just the CancellationTokenSource, nothing else.
                CancellationTokenSource dummyEntry;

                if (!_memoryCache.TryGetValue(dummyKey, out dummyEntry) || _memoryCache.TryGetValue(dummyKey, out dummyEntry) && dummyEntry.IsCancellationRequested)
                {
                    dummyEntry = _memoryCache.Set(dummyKey, new CancellationTokenSource(), dummyOptions);
                }

                if (dummyEntry != null)
                {
                    // Subscribe the main entry to dummy entry's cancellation token.
                    entryOptions.AddExpirationToken(new CancellationChangeToken(dummyEntry.Token));
                }
            }

            _memoryCache.Set(StringHelpers.Join(identifierTokens), value, entryOptions);
        }

        public void InvalidateEntry(IdentifierSet identifiers)
        {
            var typeIdentifiers = new List<string>();

            // Aggregate several types that appear in webhooks into one.
            if (identifiers.Type.Equals(CacheHelper.CONTENT_ITEM_TYPE_CODENAME, StringComparison.Ordinal) || identifiers.Type.Equals(CacheHelper.CONTENT_ITEM_VARIANT_TYPE_CODENAME, StringComparison.Ordinal))
            {
                typeIdentifiers.AddRange(new[] { CacheHelper.CONTENT_ITEM_TYPE_CODENAME, string.Join(string.Empty, CacheHelper.CONTENT_ITEM_TYPE_CODENAME, "_variant"), string.Join(string.Empty, CacheHelper.CONTENT_ITEM_TYPE_CODENAME, "_typed"), string.Join(string.Empty, CacheHelper.CONTENT_ITEM_TYPE_CODENAME, "_runtime_typed") });
            }
            else if (identifiers.Type.Equals(CacheHelper.CONTENT_ITEM_LISTING_IDENTIFIER, StringComparison.Ordinal))
            {
                typeIdentifiers.AddRange(new[] { string.Join(string.Empty, CacheHelper.CONTENT_ITEM_LISTING_IDENTIFIER, "_typed"), string.Join(string.Empty, CacheHelper.CONTENT_ITEM_LISTING_IDENTIFIER, "_runtime_typed") });
            }
            else
            {
                typeIdentifiers.Add(identifiers.Type);
            }

            foreach (var typeIdentifier in typeIdentifiers)
            {
                if (_memoryCache.TryGetValue(StringHelpers.Join("dummy", typeIdentifier, identifiers.Codename), out CancellationTokenSource dummyEntry))
                {
                    // Mark all subscribers to the CancellationTokenSource as invalid.
                    dummyEntry.Cancel();
                }
            }
        }

        /// <summary>
        /// The <see cref="IDisposable.Dispose"/> implementation.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region "Non-public methods"

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _memoryCache.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}