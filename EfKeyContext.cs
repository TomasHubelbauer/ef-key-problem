using Microsoft.EntityFrameworkCore;
using ef_key_problem.Models;

namespace ef_key_problem {
	public class EfKeyContext: DbContext {
		public EfKeyContext(DbContextOptions<EfKeyContext> options): base(options) {}
		public DbSet<Todo> Todos { get; set; }
	}
}
