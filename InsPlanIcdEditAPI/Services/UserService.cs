using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;
using InsPlanIcdEditApi.Repositories;

namespace InsPlanIcdEditApi.Services
{
    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepo)
        {
            UserRepo = userRepo;
        }
        private IUserRepository UserRepo = null;

        public UserObject GetUser(string username)
        {
            return UserRepo.GetUser(username);
        }
    }
}