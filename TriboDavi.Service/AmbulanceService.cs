using Common.DTO;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace TriboDavi.Service
{
    public class AmbulanceService : IAmbulanceService
    {
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly IChecklistRepository _checklistRepository;

        public AmbulanceService(IAmbulanceRepository ambulanceRepository, IChecklistRepository checklistRepository)
        {
            _ambulanceRepository = ambulanceRepository;
            _checklistRepository = checklistRepository;
        }

        public async Task<ResponseDTO> Create(AmbulanceDTO ambulanceDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ambulanceExists = await _ambulanceRepository.GetEntities().AnyAsync(c => c.Number == ambulanceDTO.Number);
                if (ambulanceExists)
                {
                    responseDTO.SetBadInput($"A ambulância {ambulanceDTO.Number} já existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == ambulanceDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {ambulanceDTO.IdChecklist} não existe!");
                    return responseDTO;
                }

                var ambulance = new Ambulance
                {
                    Number = ambulanceDTO.Number,
                    Checklist = checklist,
                };
                ambulance.SetCreatedAt();
                await _ambulanceRepository.InsertAsync(ambulance);
                await _ambulanceRepository.SaveChangesAsync();
                responseDTO.Object = ambulance;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, AmbulanceDTO ambulanceDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ambulance = await _ambulanceRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância {ambulanceDTO.Number} não existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == ambulanceDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {ambulanceDTO.IdChecklist} não existe!");
                    return responseDTO;
                }

                ambulance.Number = ambulanceDTO.Number;
                ambulance.SetUpdatedAt();
                ambulance.Checklist = checklist;

                await _ambulanceRepository.SaveChangesAsync();
                responseDTO.Object = ambulance;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Remove(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ambulance = await _ambulanceRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância com id: {id} não existe!");
                    return responseDTO;
                }
                _ambulanceRepository.Delete(ambulance);
                await _ambulanceRepository.SaveChangesAsync();
                responseDTO.Object = ambulance;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetList()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _ambulanceRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}