using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Test.API.Models;
using Task = Test.API.Models.Task;

namespace Test.API.DAL;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }
    
    public DbSet<Language> Language { get; set; }
    public DbSet<Task> Task { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Record> Record { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }
}