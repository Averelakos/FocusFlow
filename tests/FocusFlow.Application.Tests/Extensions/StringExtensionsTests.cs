using FluentAssertions;
using Xunit;

namespace FocusFlow.Application.Tests.Extensions;

/// <summary>
/// Tests for string extension methods
/// </summary>
public class StringExtensionsTests
{
    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user@domain.co.uk", true)]
    [InlineData("name+tag@email.com", true)]
    [InlineData("username", false)]
    [InlineData("test.domain.com", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsEmail_ShouldReturnCorrectResult(string input, bool expected)
    {
        // Act
        var result = input.IsEmail();

        // Assert
        result.Should().Be(expected);
    }
}
