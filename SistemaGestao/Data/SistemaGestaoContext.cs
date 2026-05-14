using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaGestao;

namespace SistemaGestao.Data
{
    public class SistemaGestaoContext : DbContext
    {
        public SistemaGestaoContext (DbContextOptions<SistemaGestaoContext> options)
            : base(options)
        {
        }

        public DbSet<SistemaGestao.Tarefa> Tarefa { get; set; } = default!;
    }
}
