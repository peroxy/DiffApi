using System;
using System.Linq;
using System.Net.Http;
using DiffApi.Controllers;
using DiffApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiffApi.Tests
{
    [TestClass]
    public class UnitTests
    {
        private const string Sample1 = "AAAAAA==";
        private const string Sample2 = "AQABAQ==";
        private const string Sample3 = "AAA==";

        [TestMethod]
        public void Encoding()
        {
            var comparer = new JsonComparer
            {
                Id = 1,
                Left = Sample2,
                Right = Sample1
            };
            Assert.AreEqual(comparer.Left, "\u0001\0\u0001\u0001");
            Assert.AreEqual(comparer.Right, "\0\0\0\0");
        }

        [TestMethod]
        public void Equality()
        {
            var comparer = new JsonComparer
            {
                Id = 1,
                Left = Sample1,
                Right = Sample1
            };

            Result result = comparer.GetResult();
            Assert.AreEqual(1, comparer.Id);
            Assert.IsNull(result.Differences);
            Assert.AreEqual(ResultType.Equals, result.Type);
        }

        [TestMethod]
        public void SizeMatch()
        {
            var comparer = new JsonComparer
            {
                Id = 1,
                Left = Sample3,
                Right = Sample1
            };

            Result result = comparer.GetResult();
            Assert.IsNull(result.Differences);
            Assert.AreEqual(ResultType.SizeDoesNotMatch, result.Type);
        }

        [TestMethod]
        public void Differences()
        {
            var comparer = new JsonComparer
            {
                Id = 1,
                Left = Sample2,
                Right = Sample1
            };

            Result result = comparer.GetResult();
            Assert.IsNotNull(result.Differences);
            Assert.AreEqual(2, result.Differences.Count);
            Assert.AreEqual(ResultType.ContentDoesNotMatch, result.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidConstruction()
        {
            var comparer = new JsonComparer();
            comparer.GetResult();
        }

        [TestMethod]
        public void BigData()
        {
            const string text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                    Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit
                    in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt
                    mollit anim id est laborum.";
            var left = System.Text.Encoding.UTF8.GetBytes(text);
            var right = System.Text.Encoding.UTF8.GetBytes(text.Replace(" ", "_"));

            var comparer = new JsonComparer
            {
                Id = 1,
                Left = Convert.ToBase64String(left),
                Right = Convert.ToBase64String(right)
            };
            Result result = comparer.GetResult();
            Assert.IsNotNull(result.Differences);
            Assert.AreEqual(68, result.Differences.Count);
            Assert.AreEqual(ResultType.ContentDoesNotMatch, result.Type);
        }

    }
}
