using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;

namespace InsPlanIcdEditApi.Repositories
{
    public interface IUserRepository
    {
        UserObject GetUser(string username);
    }
}