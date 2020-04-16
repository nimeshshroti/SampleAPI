using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Helper;
using SampleAPI.Model;

namespace SampleAPI.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context) {
            _context = context;
        }
        public void Add<T>(T Entity) where T : class
        {
            _context.Add(Entity);
        }

        public void Delete<T>(T Entity) where T : class
        {
            _context.Remove(Entity);
        }

        public async Task<User> GetUser(int Id)
        {
            var User = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == Id);
            return User;
        }

        public async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _context.Users.Include(x=>x.Likers).Include(x=>x.Likees).FirstOrDefaultAsync(u=> u.Id==id);

            if (likers)
            {
                return user.Likers.Where(u=> u.LikeeId==id).Select(i=> i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var Users = _context.Users.Include(p => p.Photos).OrderByDescending(u=> u.LastActive).AsQueryable();

            Users = Users.Where(u => u.Id != userParams.UserId);
            Users = Users.Where(u => u.Gender == userParams.Gender);

            if(userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                Users = Users.Where(u=> userLikees.Contains(u.Id));
            }

            if(userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                Users = Users.Where(u => userLikers.Contains(u.Id));
            }

            if(userParams.MinAge  != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                Users = Users.Where(u=> u.DateOfBirth>=minDob && u.DateOfBirth<= maxDob);
            }

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy.ToLower())
                {
                    case "created":
                       Users = Users.OrderByDescending(u => u.Created);
                            break;
                    case "lastactive":
                       Users = Users.OrderByDescending(u => u.LastActive);
                            break;
                }
            }

            return await PagedList<User>.CreateAsync(Users, userParams.PageNumber, userParams.PageSize);            
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<Photo> GetPhoto(int Id)
        {
            var Photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == Id);

            return Photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u=>u.UserId==userId).FirstOrDefaultAsync(p=>p.isMain);
            
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u=>u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<Message> GetMessage(int Id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(u=>u.Sender).ThenInclude(p=>p.Photos)
                .Include(u=>u.Receiver).ThenInclude(p=>p.Photos).AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u=>u.ReceiverId == messageParams.UserId && u.ReceipientDeleted==false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.ReceiverId == messageParams.UserId && u.IsRead==false && u.ReceipientDeleted == false);
                    break;
            }

            messages = messages.OrderBy(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
               .Include(u => u.Receiver).ThenInclude(p => p.Photos)
               .Where(m => m.ReceiverId == userId && m.SenderId == recipientId && m.ReceipientDeleted==false
               || m.ReceiverId == recipientId && m.SenderId == userId && m.SenderDeleted==false)
               .OrderByDescending(m=>m.MessageSent).ToListAsync();

            return messages;
              
        }
    }
}
