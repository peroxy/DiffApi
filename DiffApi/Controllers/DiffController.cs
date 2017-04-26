using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using DiffApi.Models;

namespace DiffApi.Controllers
{
    /// <summary>
    /// Main controller used for the Diff web api.
    /// </summary>
    public class DiffController : ApiController
    {
        /// <summary>
        /// Stores JSON data storage that does not expire for 1 day.
        /// </summary>
        private static MemoryCache _cache = MemoryCache.Default;

        private enum Direction
        {
            Left,
            Right
        }

        /// <summary>
        /// Fetches the specified ID's comparison from cache.
        /// </summary>
        /// <param name="id"></param>
        /// GET /v1/diff/id
        public HttpResponseMessage GetDiff(int id)
        {
            var result = (JsonComparer) _cache.Get(id.ToString());
            if (result?.Left == null || result.Right == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result.GetResult());
        }

        public HttpResponseMessage PutLeft(int id, HttpRequestMessage request)
        {
            return AddToCache(id, request, Direction.Left);
        }

        public HttpResponseMessage PutRight(int id, HttpRequestMessage request)
        {
            return AddToCache(id, request, Direction.Right);
        }

        /// <summary>
        /// Clears the entire cache, disposes of it and creates a new default one.
        /// </summary>
        public void ClearCache()
        {
            _cache.Dispose();
            _cache = MemoryCache.Default;
        }

        /// <summary>
        /// Creates a JSON comparison and saves the specified ID and comparison into cache.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private HttpResponseMessage AddToCache(int id, HttpRequestMessage request, Direction direction)
        {
            string json = request.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(json))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) };
            JsonComparer comparer;
            if (_cache.Contains(id.ToString()))
            {
                comparer = (JsonComparer)_cache.Get(id.ToString());
                ConstructComparer(direction, json, comparer);
                _cache.Set(id.ToString(), comparer, policy);
            }
            else
            {
                comparer = new JsonComparer {Id = id};
                ConstructComparer(direction, json, comparer);
                _cache.Add(id.ToString(), comparer, policy);
            }
            return Request.CreateResponse(HttpStatusCode.Created);

        }

        private static void ConstructComparer(Direction direction, string json, JsonComparer comparer)
        {
            switch (direction)
            {
                case Direction.Left:
                    comparer.Left = json;
                    break;
                case Direction.Right:
                    comparer.Right = json;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid direction.");
            }
        }
    }
}
