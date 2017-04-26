using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiffApi.Models
{
    /// <summary>
    /// JSON comparison helper class used to determine the differences between the two specified JSON strings.
    /// </summary>
    public class JsonComparer
    {
        private string _right, _left;

        /// <summary>
        /// Base64 encoded JSON string should be supplied to be used during comparison.
        /// </summary>
        public string Left
        {
            get { return _left; }
            set { _left = Base64Decode(value); }
        }

        /// <summary>
        /// Base64 encoded JSON string should be supplied to be used during comparison.
        /// </summary>
        public string Right
        {
            get { return _right; }
            set { _right = Base64Decode(value); }
        }

        /// <summary>
        /// ID used for memory caching.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Executes the comparison and returns the final differences.
        /// </summary>
        /// <returns></returns>
        public Result GetResult()
        {
            if (_left == null || _right == null)
            {
                throw new InvalidOperationException("Cannot fetch result without both JSON values defined.");
            }

            var result = new Result();

            if (_left == _right)
            {
                result.Type = ResultType.Equals;
                return result;
            }

            if (_left.Length != _right.Length)
            {
                result.Type = ResultType.SizeDoesNotMatch;
                return result;
            }

            HandleNotMatching(result);

            return result;
        }

        /// <summary>
        /// Handles not matching content and fills up the final result's differences.
        /// </summary>
        /// <param name="result"></param>
        private void HandleNotMatching(Result result)
        {
            result.Type = ResultType.ContentDoesNotMatch;
            result.Differences = new List<Difference>();
            int length = 0;
            int offset = 0;
            bool found = false;
            for (int i = 0; i < _left.Length; i++)
            {
                if (_left[i] == _right[i])
                {
                    if (length > 0)
                    {
                        result.Differences.Add(new Difference(offset, length));
                        length = 0;
                        found = false;
                    }
                }
                else
                {
                    length++;
                    if (!found)
                    {
                        offset = i;
                        found = true;
                    }
                }
            }

            // handle edge case if the last character was a difference and did not get stored into result
            if (found)
            {
                result.Differences.Add(new Difference(offset, length));
            }
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}