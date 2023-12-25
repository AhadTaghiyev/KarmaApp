using System;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Karma.Service.Services.Implementations
{
    public class PositionService : IPositionService
    {
        readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository PositionRepository)
        {
            _positionRepository = PositionRepository;
        }

        public async Task CreateAsync(PositionPostDto dto)
        {
            Position Position = new Position();
            Position.Name = dto.Name;
            await _positionRepository.AddAsync(Position);
            await _positionRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PositionGetDto>> GetAllAsync()
        {
            IEnumerable<PositionGetDto> Positions = await _positionRepository.GetQuery(x => !x.IsDeleted)
               .AsNoTrackingWithIdentityResolution().Select(x => new PositionGetDto { Name = x.Name, Id = x.Id, CreatedAt = x.CreatedAt })
               .ToListAsync();
            return Positions;
        }

        public async Task<PositionGetDto> GetAsync(int id)
        {
            Position? Position = await _positionRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Position == null)
            {
                throw new ItemNotFoundExcpetion("Position Not Found");
            }

            PositionGetDto PositionGetDto = new PositionGetDto
            {
                CreatedAt = Position.CreatedAt,
                Id = Position.Id,
                Name = Position.Name
            };
            return PositionGetDto;
        }

        public async Task RemoveAsync(int id)
        {
            Position? Position = await _positionRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Position == null)
            {
                throw new ItemNotFoundExcpetion("Position Not Found");
            }
            Position.IsDeleted = true;
            await _positionRepository.UpdateAsync(Position);
            await _positionRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PositionPostDto dto)
        {
            Position? Position = await _positionRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Position == null)
            {
                throw new ItemNotFoundExcpetion("Position Not Found");
            }

            Position.Name = dto.Name;
            await _positionRepository.UpdateAsync(Position);
            await _positionRepository.SaveChangesAsync();
        }
    }
}

