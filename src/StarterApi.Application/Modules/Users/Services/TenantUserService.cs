using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Users.DTOs;

using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Users.Services
{
    public class TenantUserService : ITenantUserService
    {
        private readonly ITenantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TenantUserService> _logger;

        public TenantUserService(
            ITenantDbContext context,
            IMapper mapper,
            ILogger<TenantUserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new TenantUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            return _mapper.Map<UserDto>(user);
        }
    }
} 