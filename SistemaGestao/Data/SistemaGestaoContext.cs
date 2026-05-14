using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaGestao.Models;

namespace SistemaGestao.Data
{
    public class SistemaGestaoContext : DbContext
    {
        public SistemaGestaoContext (DbContextOptions<SistemaGestaoContext> options)
            : base(options)
        {
        }

        public DbSet<Tarefa> Tarefa { get; set; } = default!;
    }
}
