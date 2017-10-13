using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerFetch
{
	public class PlayerContext : DbContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<Score> Scores { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql("server=localhost;userid=root;pwd=;port=3306;database=osu_players;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Player>(builder =>
			{
				builder.HasIndex(p => p.total_score);
			});

			modelBuilder.Entity<Score>(builder =>
			{
				builder.HasOne(p => p.player)
				.WithMany(s => s.Scores)
				.HasForeignKey(s => s.user_id);

				builder.HasKey(s => new {s.beatmap_id, s.user_id, s.enabled_mods});
			});

		}

	}
}
