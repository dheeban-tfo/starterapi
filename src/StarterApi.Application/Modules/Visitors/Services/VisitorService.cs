using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Visitors.DTOs;
using StarterApi.Application.Modules.Visitors.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Application.Modules.Visitors.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly IVisitorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public VisitorService(
            IVisitorRepository repository, 
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<VisitorListDto>> GetVisitorsAsync(QueryParameters parameters)
        {
            var visitors = await _repository.GetPagedAsync(parameters);
            
            return new PagedResult<VisitorListDto>
            {
                Items = _mapper.Map<IEnumerable<VisitorListDto>>(visitors.Items),
                TotalItems = visitors.TotalItems,
                PageNumber = visitors.PageNumber,
                PageSize = visitors.PageSize,
                TotalPages = visitors.TotalPages,
                HasNextPage = visitors.HasNextPage,
                HasPreviousPage = visitors.HasPreviousPage
            };
        }

        public async Task<VisitorDto> GetByIdAsync(Guid id)
        {
            var visitor = await _repository.GetByIdAsync(id);
            if (visitor == null)
                throw new NotFoundException(nameof(Visitor), id);

            return _mapper.Map<VisitorDto>(visitor);
        }

        public async Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto dto)
        {
            var currentUserId = _currentUserService.UserId;
            if (!currentUserId.HasValue)
                throw new UnauthorizedException("User is not authenticated");

            var visitor = new Visitor
            {
                VisitorName = dto.VisitorName,
                ExpectedVisitDate = dto.ExpectedVisitDate,
                ExpectedVisitStartTime = dto.ExpectedVisitStartTime,
                ExpectedVisitEndTime = dto.ExpectedVisitEndTime,
                PurposeOfVisit = dto.PurposeOfVisit,
                ResidentId = currentUserId.Value,
                IsParking = dto.IsParking,
                Status = "Pending"
            };

            visitor = await _repository.CreateAsync(visitor);
            return _mapper.Map<VisitorDto>(visitor);
        }

        public async Task<VisitorDto> UpdateVisitorAsync(Guid id, UpdateVisitorDto dto)
        {
            var visitor = await _repository.GetByIdAsync(id);
            if (visitor == null)
                throw new NotFoundException(nameof(Visitor), id);

            visitor.VisitorName = dto.VisitorName;
            visitor.ExpectedVisitDate = dto.ExpectedVisitDate;
            visitor.ExpectedVisitStartTime = dto.ExpectedVisitStartTime;
            visitor.ExpectedVisitEndTime = dto.ExpectedVisitEndTime;
            visitor.PurposeOfVisit = dto.PurposeOfVisit;
            visitor.IsParking = dto.IsParking;
            
            if (!string.IsNullOrEmpty(dto.Status))
                visitor.Status = dto.Status;

            visitor = await _repository.UpdateAsync(visitor);
            return _mapper.Map<VisitorDto>(visitor);
        }

        public async Task<bool> DeleteVisitorAsync(Guid id)
        {
            if (!await _repository.ExistsAsync(id))
                throw new NotFoundException(nameof(Visitor), id);

            return await _repository.DeleteAsync(id);
        }

        public async Task<VisitorDto> UpdateVisitorStatusAsync(Guid id, UpdateVisitorStatusDto dto)
        {
            var visitor = await _repository.GetByIdAsync(id);
            if (visitor == null)
                throw new NotFoundException(nameof(Visitor), id);

            visitor.Status = dto.Status;
            visitor = await _repository.UpdateAsync(visitor);
            return _mapper.Map<VisitorDto>(visitor);
        }

        public async Task<PagedResult<VisitorListDto>> GetUpcomingVisitorsAsync(QueryParameters parameters)
        {
            var visitors = await _repository.GetUpcomingVisitorsAsync(parameters);
            
            return new PagedResult<VisitorListDto>
            {
                Items = _mapper.Map<IEnumerable<VisitorListDto>>(visitors.Items),
                TotalItems = visitors.TotalItems,
                PageNumber = visitors.PageNumber,
                PageSize = visitors.PageSize,
                TotalPages = visitors.TotalPages,
                HasNextPage = visitors.HasNextPage,
                HasPreviousPage = visitors.HasPreviousPage
            };
        }

        public async Task<PagedResult<VisitorListDto>> GetPastVisitorsAsync(QueryParameters parameters)
        {
            var visitors = await _repository.GetPastVisitorsAsync(parameters);
            
            return new PagedResult<VisitorListDto>
            {
                Items = _mapper.Map<IEnumerable<VisitorListDto>>(visitors.Items),
                TotalItems = visitors.TotalItems,
                PageNumber = visitors.PageNumber,
                PageSize = visitors.PageSize,
                TotalPages = visitors.TotalPages,
                HasNextPage = visitors.HasNextPage,
                HasPreviousPage = visitors.HasPreviousPage
            };
        }
    }
} 