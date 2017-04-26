using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DiffApi.Models
{
    /// <summary>
    /// Result class used for final differences JSON result.
    /// </summary>
    public class Result
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultType Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Difference> Differences { get; set; }
    }
}