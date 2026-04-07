using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using QMSWebApplication.BackendServer.Services;

namespace QMSWebApplication.BackendServer.UnitTest.Services
{
    public class ClientIpAccessorServiceTest
    {

        [Fact]
        public void GetClientIp_Should_Return_CF_Connecting_IP_When_Present()
        {
            // Arrange
            var ctx = new DefaultHttpContext();
            ctx.Request.Headers["CF-Connecting-IP"] = new StringValues("203.0.113.10");
            // Giả lập proxy ở RemoteIpAddress
            ctx.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.1");

            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(ctx);

            var service = new ClientIpAccessorService(accessor.Object);

            // Act
            var ip = service.GetClientIp();

            // Assert
            Assert.Equal("203.0.113.10", ip);
        }

        [Fact]
        public void GetClientIp_Should_Read_RemoteIpAddress_When_No_Forwarded_Header()
        {
            var ctx = new DefaultHttpContext();
            ctx.Connection.RemoteIpAddress = IPAddress.Parse("198.51.100.7");

            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(ctx);

            var service = new ClientIpAccessorService(accessor.Object);

            var ip = service.GetClientIp();

            Assert.Equal("198.51.100.7", ip);
        }

        [Fact]
        public void GetClientIp_Should_Parse_XFF_When_Configured_Without_ForwardedHeaders()
        {
            var ctx = new DefaultHttpContext();
            ctx.Request.Headers["X-Forwarded-For"] = new StringValues("203.0.113.20, 10.0.0.1");
            ctx.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.2");

            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(ctx);

            var service = new ClientIpAccessorService(accessor.Object, preferXff: true);

            var ip = service.GetClientIp();

            Assert.Equal("203.0.113.20", ip);
        }

        [Fact]
        public void GetClientIp_Should_Map_Localhost_To_127_0_0_1()
        {
            var ctx = new DefaultHttpContext();
            ctx.Connection.RemoteIpAddress = IPAddress.IPv6Loopback; // ::1

            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(ctx);

            var service = new ClientIpAccessorService(accessor.Object);

            var ip = service.GetClientIp();

            Assert.Equal("127.0.0.1", ip);
        }

    }
}
