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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var Users = _context.Users.Include(p => p.Photos).OrderByDescending(u=> u.LastActive).AsQueryable();

            Users = Users.Where(u => u.Id != userParams.UserId);
            Users = Users.Where(u => u.Gender == userParams.Gender);

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
    }
}
