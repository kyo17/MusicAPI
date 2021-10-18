using System;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DML
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}
