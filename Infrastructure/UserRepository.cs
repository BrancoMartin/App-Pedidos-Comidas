using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Infrastructure
{
    public class UserRepository : RepositoryBase<Usuario>, IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;
        public UserRepository(AppDbContext dbContext, IConfiguration config) : base(dbContext)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<Usuario?> GetUserByPhoneAsync(string telefono)
        {
            return await _dbContext.Usuarios

                .FirstOrDefaultAsync(u => u.Telefono == telefono);
        }
    }
}
