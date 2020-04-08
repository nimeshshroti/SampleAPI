using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<User>> GetUsers()
        {
            var Users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return Users;
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
    }
}
