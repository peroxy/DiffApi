using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiffApi.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private const string Sample1 = "AAAAAA==";
        private const string Sample2 = "AQABAQ==";

        [TestMethod]
        public void PutValid()
        {
            var controller = new Controllers.DiffController();
            var req = new HttpRequestMessage { Content = new StringContent(Sample1) };
            controller.Request = req;

            HttpResponseMessage res = controller.PutLeft(1, req);
            Assert.AreEqual(HttpStatusCode.Created, res.StatusCode);

            res = controller.PutRight(1, req);
            Assert.AreEqual(HttpStatusCode.Created, res.StatusCode);
        }

        [TestMethod]
        public void PutInvalid()
        {
            var controller = new Controllers.DiffController();
            var req = new HttpRequestMessage { Content = new StringContent("") };
            controller.Request = req;

            HttpResponseMessage res = controller.PutLeft(1, req);
            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
        }

        [TestMethod]
        public void GetValidDifferences()
        {
            var controller = new Controllers.DiffController();
            var req = new HttpRequestMessage { Content = new StringContent(Sample1) };
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            controller.Request = req;
            controller.PutLeft(1, req);

            req.Content = new StringContent(Sample2);
            controller.Request = req;
            controller.PutRight(1, req);

            HttpResponseMessage res = controller.GetDiff(1);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

        }

        [TestMethod]
        public void GetInvalidDifferences()
        {
            var controller = new Controllers.DiffController();
            controller.ClearCache();  // required in case we run all unit tests at once, that keeps the same cache throughout the entire session and needs clearing
            var req = new HttpRequestMessage { Content = new StringContent(Sample1) };
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            controller.Request = req;
            HttpResponseMessage res = controller.GetDiff(1);
            Assert.AreEqual(HttpStatusCode.NotFound, res.StatusCode);
        }
    }
}
