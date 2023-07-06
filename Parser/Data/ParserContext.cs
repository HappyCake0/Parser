using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parser.Models;

namespace Parser.Data
{
    public class ParserContext : DbContext
    {
        public ParserContext (DbContextOptions<ParserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Parser.Models.User> User { get; set; } = default!;
    }
}
