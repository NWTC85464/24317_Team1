using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    class ChatroomContext : DbContext
    {
        public virtual DbSet<ChatRoom> ChatRooms { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
