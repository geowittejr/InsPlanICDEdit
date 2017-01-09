using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsPlanIcdEditApi.Models;

namespace InsPlanIcdEditApi.Services
{
    public interface IUserService
    {
        UserObject GetUser(string username);
    }
}