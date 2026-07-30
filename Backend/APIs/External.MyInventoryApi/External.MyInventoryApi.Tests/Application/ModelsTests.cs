using External.MyInventoryApi.Application.Contracts.Results;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.Application
{
    public class ModelsTests
    {
        [Fact]
        public void Success_ShouldReturnServiceResultWithSuccessValues()
        {
            // Act
            var result = ServiceResult<int>.Success(123);

            // Assert
            result.Data.Should().Be(123);
            result.ErrorCode.Should().Be(0);
            result.ErrorMessage.Should().Be("OK");
        }

        [Fact]
        public void Fail_ShouldReturnServiceResultWithFailValues()
        {
            // Act
            var result = ServiceResult<int>.Fail(321,"Wrong value");

            // Assert
            result.Data.Should().Be(default);
            result.ErrorCode.Should().Be(321);
            result.ErrorMessage.Should().Be("Wrong value");
        }
    }
}
