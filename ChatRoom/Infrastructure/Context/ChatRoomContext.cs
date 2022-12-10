using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ChatRoomContext : DbContext
    {
        public ChatRoomContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<ChatRoomEntity> ChatRooms { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
