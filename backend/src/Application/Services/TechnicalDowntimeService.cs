using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs; 

namespace Application.Services
{
    public class TechnicalDowntimeService
    {
        private readonly ITechnicalDowntimeRepository _technicalDowntimeRepository;

        public TechnicalDowntimeService(ITechnicalDowntimeRepository technicalDowntimeRepository)
        {
            _technicalDowntimeRepository = technicalDowntimeRepository;
        }

        public async Task CreateTechnicalDowntimeAsync(TechnicalDowntimeDTO dto)
        {
            var technicalDowntime = new TechnicalDowntime
            {
                EquipmentId = dto.EquipmentId,
                TechnicalId = dto.TechnicalId,
                DepartmentId = dto.DepartmentId,
                DestinyTypeId = dto.DestinyTypeId,
                Cause = dto.Cause,
                Date = DateTime.UtcNow
            };

            await _technicalDowntimeRepository.AddAsync(technicalDowntime);
            // BaseRepository guarda cambios en AddAsync por ahora.
        }
    }
}
