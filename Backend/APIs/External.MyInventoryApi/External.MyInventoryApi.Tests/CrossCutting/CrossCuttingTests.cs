using External.MyInventoryApi.CrossCutting.HealthChecks;
using External.MyInventoryApi.CrossCutting.Middleware;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.CrossCutting
{
    public class CrossCuttingTests
    {
        /*
         *---------------------------------------------------------
         *---------------| Middlewares use cases |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task InvokeAsync_ShouldCallNext_WhenHeaderExists()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Headers["x-CorrelationId"] = "123";

            bool nextCalled = false;

            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var loggerMock = new Mock<ILogger<HeaderValidationMiddleware>>();

            var middleware = new HeaderValidationMiddleware(
                next,
                loggerMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            nextCalled.Should().BeTrue();
        }


        [Fact]
        public async Task InvokeAsync_ShouldCallNext_WhenHeaderDoesNotExist()
        {
            // Arrange
            var context = new DefaultHttpContext();

            bool nextCalled = false;

            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var loggerMock = new Mock<ILogger<HeaderValidationMiddleware>>();

            var middleware = new HeaderValidationMiddleware(
                next,
                loggerMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            nextCalled.Should().BeTrue();
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotThrow_WhenNextThrowsException()
        {
            // Arrange
            var context = new DefaultHttpContext();

            RequestDelegate next = _ => throw new Exception("Test");

            var loggerMock = new Mock<ILogger<HeaderValidationMiddleware>>();

            var middleware = new HeaderValidationMiddleware(
                next,
                loggerMock.Object);

            // Act
            Func<Task> act = () => middleware.InvokeAsync(context);

            // Assert
            await act.Should().NotThrowAsync();
        }
        /*
         *---------------------------------------------------------
         *---------------| CheckHealthAsync use case |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task CheckHealthAsync_ShouldReturnHealthy_WhenDatabaseIsHealthy()
        {
            // Arrange
            var databaseMock = new Mock<ISqlServerDatabase>();

            databaseMock
                .Setup(d => d.IsHealthyAsync())
                .ReturnsAsync(true);

            var healthCheck = new SqlServerCustomHealthCheck(databaseMock.Object);

            // Act
            var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenDatabaseIsNotHealthy()
        {
            // Arrange
            var databaseMock = new Mock<ISqlServerDatabase>();

            databaseMock
                .Setup(d => d.IsHealthyAsync())
                .ReturnsAsync(false);

            var healthCheck = new SqlServerCustomHealthCheck(databaseMock.Object);

            // Act
            var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenExceptionOccurs()
        {
            // Arrange
            var databaseMock = new Mock<ISqlServerDatabase>();

            databaseMock
                .Setup(d => d.IsHealthyAsync())
                .ThrowsAsync(new Exception());

            var healthCheck = new SqlServerCustomHealthCheck(databaseMock.Object);

            // Act
            var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
