using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carwash.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carwash.Model;

namespace carwash.Controllers
{
    public class LoginController :ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        public LoginController(ApplicationDbContext dbContext)
        {
                _dbcontext = dbContext;
        }

       
    }
}