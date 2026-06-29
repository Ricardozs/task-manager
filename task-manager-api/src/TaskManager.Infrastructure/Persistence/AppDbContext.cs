using Microsoft.EntityFrameworkCore;

namespace TaskManager.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
