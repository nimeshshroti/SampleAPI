using Newtonsoft.Json;
using SampleAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Data
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext dataContext ) {
            _dataContext = dataContext;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public void SeedUsers()
        {
            var UserData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var Users = JsonConvert.DeserializeObject<List<User>>(UserData);

            foreach(var User in Users)
            {
                byte[] PasswordHash, PasswordSalt;
                CreatePasswordHash("password", out PasswordHash, out PasswordSalt);

                User.PasswordHash = PasswordHash;
                User.PasswordSalt = PasswordSalt;
                User.Username = User.Username.ToLower();

                _dataContext.Users.Add(User);
            }

            _dataContext.SaveChanges();

        }
    }
}
