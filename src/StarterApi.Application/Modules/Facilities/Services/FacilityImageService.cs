using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using AutoMapper;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Domain.Constants;

namespace StarterApi.Application.Modules.Facilities.Services
{
    public class FacilityImageService : IFacilityImageService
    {
        private readonly IFacilityImageRepository _facilityImageRepository;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        private readonly ILogger<FacilityImageService> _logger;

        public FacilityImageService(
            IFacilityImageRepository facilityImageRepository,
            IDocumentService documentService,
            IMapper mapper,
            ILogger<FacilityImageService> logger)
        {
            _facilityImageRepository = facilityImageRepository;
            _documentService = documentService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FacilityImageDto> UploadImageAsync(CreateFacilityImageDto dto)
        {
            try
            {
                // Create document first
                using var stream = dto.File.OpenReadStream();
                var document = await _documentService.UploadDocumentAsync(
                    dto.File.FileName,
                    stream,
                    dto.File.ContentType,
                    null, // categoryId
                    null, // unitId
                    null  // blockId
                );

                // Create facility image
                var facilityImage = new FacilityImage
                {
                    FacilityId = dto.FacilityId,
                    DocumentId = document.Id,
                    IsPrimary = dto.IsPrimary,
                    DisplayOrder = dto.DisplayOrder
                };

                await _facilityImageRepository.AddAsync(facilityImage);
                await _facilityImageRepository.SaveChangesAsync();

                // If this is primary, update other images
                if (dto.IsPrimary)
                {
                    await UpdatePrimaryImageAsync(facilityImage.Id, dto.FacilityId);
                }

                // Map to DTO with document properties
                var result = _mapper.Map<FacilityImageDto>(facilityImage);
                result.FileName = document.Name;
                result.ContentType = document.ContentType;
                result.FileSize = document.Size;
                result.Description = document.Description;
                result.DownloadUrl = $"/api/FacilityImages/{facilityImage.Id}/content"; // Update to use our proxy endpoint

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading facility image");
                throw;
            }
        }

        public async Task<ImageContentDto> GetImageContentAsync(Guid id)
        {
            var facilityImage = await _facilityImageRepository.GetByIdAsync(id);
            if (facilityImage == null)
                throw new NotFoundException($"Facility image with ID {id} not found");

            var document = await _documentService.GetDocumentByIdAsync(facilityImage.DocumentId);
            if (document == null)
                throw new NotFoundException($"Document not found for facility image {id}");

            var content = await _documentService.GetDocumentContentAsync(document.Id);
            
            return new ImageContentDto
            {
                Content = content,
                ContentType = document.ContentType
            };
        }

        public async Task<FacilityImageDto> UpdateImageAsync(Guid id, UpdateFacilityImageDto dto)
        {
            var facilityImage = await _facilityImageRepository.GetByIdAsync(id);
            if (facilityImage == null)
                throw new NotFoundException($"Facility image with ID {id} not found");

            // Update document if new file is provided
            if (dto.File != null)
            {
                using var stream = dto.File.OpenReadStream();
                await _documentService.UpdateDocumentAsync(
                    facilityImage.DocumentId,
                    dto.File.FileName,
                    stream,
                    dto.File.ContentType
                );
            }

            // Update facility image properties
            facilityImage.IsPrimary = dto.IsPrimary;
            facilityImage.DisplayOrder = dto.DisplayOrder;

            await _facilityImageRepository.SaveChangesAsync();

            // If this is primary, update other images
            if (dto.IsPrimary)
            {
                await UpdatePrimaryImageAsync(id, facilityImage.FacilityId);
            }

            // Get updated document
            var document = await _documentService.GetDocumentByIdAsync(facilityImage.DocumentId);

            // Map to DTO with document properties
            var result = _mapper.Map<FacilityImageDto>(facilityImage);
            result.FileName = document.Name;
            result.ContentType = document.ContentType;
            result.FileSize = document.Size;
            result.Description = document.Description;
            result.DownloadUrl = $"/api/FacilityImages/{facilityImage.Id}/content"; // Update to use our proxy endpoint

            return result;
        }

        public async Task<bool> DeleteImageAsync(Guid id)
        {
            var facilityImage = await _facilityImageRepository.GetByIdAsync(id);
            if (facilityImage == null)
                return false;

            // Delete document first
            await _documentService.DeleteDocumentAsync(facilityImage.DocumentId);

            // Delete facility image
            await _facilityImageRepository.DeleteAsync(facilityImage);
            await _facilityImageRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<FacilityImageDto>> GetByFacilityIdAsync(Guid facilityId)
        {
            var images = await _facilityImageRepository.GetByFacilityIdAsync(facilityId);
            var result = new List<FacilityImageDto>();

            foreach (var image in images)
            {
                var document = await _documentService.GetDocumentByIdAsync(image.DocumentId);
                var dto = _mapper.Map<FacilityImageDto>(image);
                dto.FileName = document.Name;
                dto.ContentType = document.ContentType;
                dto.FileSize = document.Size;
                dto.Description = document.Description;
                dto.DownloadUrl = $"/api/FacilityImages/{image.Id}/content"; // Update to use our proxy endpoint
                result.Add(dto);
            }

            return result;
        }

        private async Task UpdatePrimaryImageAsync(Guid currentImageId, Guid facilityId)
        {
            var images = await _facilityImageRepository.GetByFacilityIdAsync(facilityId);
            foreach (var image in images)
            {
                if (image.Id != currentImageId && image.IsPrimary)
                {
                    image.IsPrimary = false;
                    await _facilityImageRepository.SaveChangesAsync();
                }
            }
        }
    }
} 