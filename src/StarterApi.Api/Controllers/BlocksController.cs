using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BlocksController : ControllerBase
    {
        private readonly IBlockService _blockService;
        private readonly ILogger<BlocksController> _logger;

        public BlocksController(
            IBlockService blockService,
            ILogger<BlocksController> logger)
        {
            _blockService = blockService;
            _logger = logger;
        }

        [HttpPost]
        [RequirePermission(Permissions.Blocks.Create)]
        public async Task<ActionResult<BlockDto>> CreateBlock(CreateBlockDto dto)
        {
            try
            {
                var block = await _blockService.CreateBlockAsync(dto);
                return CreatedAtAction(nameof(GetBlock), new { id = block.Id }, block);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating block");
                return StatusCode(500, "An error occurred while creating the block");
            }
        }

        [HttpGet]
        [RequirePermission(Permissions.Blocks.View)]
        public async Task<ActionResult<PagedResult<BlockListDto>>> GetBlocks([FromQuery] QueryParameters parameters)
        {
            try
            {
                var blocks = await _blockService.GetBlocksAsync(parameters);
                return Ok(blocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blocks");
                return StatusCode(500, "An error occurred while retrieving blocks");
            }
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Blocks.View)]
        public async Task<ActionResult<BlockDto>> GetBlock(Guid id)
        {
            try
            {
                var block = await _blockService.GetBlockByIdAsync(id);
                return Ok(block);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving block");
                return StatusCode(500, "An error occurred while retrieving the block");
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Blocks.Edit)]
        public async Task<ActionResult<BlockDto>> UpdateBlock(Guid id, UpdateBlockDto dto)
        {
            try
            {
                var block = await _blockService.UpdateBlockAsync(id, dto);
                return Ok(block);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating block");
                return StatusCode(500, "An error occurred while updating the block");
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Blocks.Delete)]
        public async Task<ActionResult<bool>> DeleteBlock(Guid id)
        {
            try
            {
                var result = await _blockService.DeleteBlockAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting block");
                return StatusCode(500, "An error occurred while deleting the block");
            }
        }
    }
} 