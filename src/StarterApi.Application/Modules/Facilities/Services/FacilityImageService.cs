using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Services
{
    public class FacilityImageService : IFacilityImageService
    {
        private readonly IFacilityImageRepository _repository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;

        public FacilityImageService(
            IFacilityImageRepository repository,
            IFacilityRepository facilityRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper)
        {
            _repository = repository;
            _facilityRepository = facilityRepository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FacilityImageDto>> GetByFacilityIdAsync(Guid facilityId)
        {
            var images = await _repository.GetByFacilityIdAsync(facilityId);
            return _mapper.Map<IEnumerable<FacilityImageDto>>(images);
        }

        public async Task<FacilityImageDto> GetByIdAsync(Guid id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null)
            {
                throw new NotFoundException(nameof(FacilityImage), id);
            }

            return _mapper.Map<FacilityImageDto>(image);
        }

        public async Task<FacilityImageDto> UploadAsync(Guid facilityId, IFormFile file, CreateFacilityImageDto dto)
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), facilityId);
            }

            // Upload file to blob storage
            var blobName = $"facilities/{facilityId}/{Guid.NewGuid()}-{file.FileName}";
            var filePath = await _blobStorageService.UploadAsync(blobName, file.ContentType, file.OpenReadStream(), file.FileName);

            var image = new FacilityImage
            {
                FacilityId = facilityId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                FilePath = filePath.blobUrl,
                FileSize = file.Length,
                Description = dto.Description,
                IsPrimary = dto.IsPrimary,
                DisplayOrder = dto.DisplayOrder
            };

            // If this is set as primary, update other images
            if (dto.IsPrimary)
            {
                var existingImages = await _repository.GetByFacilityIdAsync(facilityId);
                foreach (var existingImage in existingImages)
                {
                    existingImage.IsPrimary = false;
                }
                await _repository.UpdateRangeAsync(existingImages);
            }

            await _repository.AddAsync(image);
            return _mapper.Map<FacilityImageDto>(image);
        }

        public async Task<FacilityImageDto> UpdateAsync(Guid id, UpdateFacilityImageDto dto)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null)
            {
                throw new NotFoundException(nameof(FacilityImage), id);
            }

            // If this is set as primary, update other images
            if (dto.IsPrimary && !image.IsPrimary)
            {
                var existingImages = await _repository.GetByFacilityIdAsync(image.FacilityId);
                foreach (var existingImage in existingImages)
                {
                    existingImage.IsPrimary = false;
                }
                await _repository.UpdateRangeAsync(existingImages);
            }

            image.Description = dto.Description;
            image.IsPrimary = dto.IsPrimary;
            image.DisplayOrder = dto.DisplayOrder;

            await _repository.UpdateAsync(image);
            return _mapper.Map<FacilityImageDto>(image);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null)
            {
                throw new NotFoundException(nameof(FacilityImage), id);
            }

            // Delete from blob storage
            await _blobStorageService.DeleteAsync("facilities", image.FilePath);

            return await _repository.DeleteAsync(image);
        }

        public async Task<FacilityImageDto> SetPrimaryImageAsync(Guid facilityId, Guid imageId)
        {
            var images = await _repository.GetByFacilityIdAsync(facilityId);
            var selectedImage = images.FirstOrDefault(i => i.Id == imageId);

            if (selectedImage == null)
            {
                throw new NotFoundException(nameof(FacilityImage), imageId);
            }

            foreach (var image in images)
            {
                image.IsPrimary = image.Id == imageId;
            }

            await _repository.UpdateRangeAsync(images);
            return _mapper.Map<FacilityImageDto>(selectedImage);
        }

        public async Task<bool> ReorderImagesAsync(Guid facilityId, IEnumerable<Guid> imageIds)
        {
            var images = await _repository.GetByFacilityIdAsync(facilityId);
            var imagesList = images.ToList();
            var displayOrder = 0;

            foreach (var id in imageIds)
            {
                var image = imagesList.FirstOrDefault(i => i.Id == id);
                if (image != null)
                {
                    image.DisplayOrder = displayOrder++;
                }
            }

            return await _repository.UpdateRangeAsync(imagesList);
        }
    }
} 