using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Identity;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Tests.Persistence;

public class DemoDataSeederTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly PasswordHasher _passwordHasher = new();

    public DemoDataSeederTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
    }

    [Fact]
    public async Task SeedAsync_creates_demo_user_and_tasks_when_database_is_empty()
    {
        var seeded = await DemoDataSeeder.SeedAsync(_db, _passwordHasher);

        seeded.Should().BeTrue();

        var user = await _db.Users.SingleAsync();
        user.Email.Should().Be(DemoDataSeeder.DemoEmail);
        _passwordHasher.Verify(DemoDataSeeder.DemoPassword, user.PasswordHash).Should().BeTrue();

        var tasks = await _db.Tasks.ToListAsync();
        tasks.Should().HaveCount(DemoDataSeeder.DemoTaskCount);
        tasks.Should().OnlyContain(task => task.UserId == user.Id);
    }

    [Fact]
    public async Task SeedAsync_is_idempotent_when_users_already_exist()
    {
        await DemoDataSeeder.SeedAsync(_db, _passwordHasher);

        var seeded = await DemoDataSeeder.SeedAsync(_db, _passwordHasher);

        seeded.Should().BeFalse();
        (await _db.Users.CountAsync()).Should().Be(1);
        (await _db.Tasks.CountAsync()).Should().Be(DemoDataSeeder.DemoTaskCount);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}
