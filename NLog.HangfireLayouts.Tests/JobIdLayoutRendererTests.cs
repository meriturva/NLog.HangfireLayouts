﻿using Hangfire.PerformContextAccessor;
using HangfireNLog.NLog;
using NLog.HangfireLayouts.Tests.Mocks;
using Xunit;

namespace NLog.HangfireLayouts.Tests
{
    public class JobIdLayoutRendererTests
    {
        private readonly PerformContextMock _context;
        private readonly PerformContextAccessor _performContextAccessor;

        public JobIdLayoutRendererTests()
        {
            _context = new PerformContextMock();
            _performContextAccessor = new PerformContextAccessor();
        }

        [Fact]
        public void SuccessTest()
        {
            // Arrange
            _performContextAccessor.PerformingContext = _context.Object;
            var renderer = new JobIdLayoutRenderer();
            renderer.PerformContextAccessor = _performContextAccessor;
            // Act
            string result = renderer.Render(new LogEventInfo());
            // Assert
            Assert.Equal("JobId", result);
        }

        [Fact]
        public void EmptyTest()
        {
            // Arrange
            var renderer = new JobIdLayoutRenderer();
            renderer.PerformContextAccessor = _performContextAccessor;
            // Act
            string result = renderer.Render(new LogEventInfo());
            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void NoPerformContextAccessorTest()
        {
            // Arrange
            var renderer = new JobIdLayoutRenderer();
            renderer.PerformContextAccessor = null;
            // Act
            string result = renderer.Render(new LogEventInfo());
            // Assert
            Assert.Equal("", result);
        }
    }
}