using Aula02Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aula02Api.Db
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<User> Users { get; set; }
    }
}