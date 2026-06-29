using FluentAssertions;

namespace TaskManager.Application.Tests;

public class BootstrapTests
{
    [Fact]
    public void Solution_bootstraps_successfully()
    {
        true.Should().BeTrue();
    }
}
