using Common.DTO;
using Common.Functions;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain.Enum;
using TriboDavi.Domain.Identity;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TriboDavi.Domain;
using AutoMapper;

namespace TriboDavi.Service
{
    public class LegalParentService : ILegalParentService
    {
        private readonly ILegalParentRepository _legalParentRepository;
        private readonly IMapper _mapper;
        public LegalParentService(ILegalParentRepository legalParentRepository, IMapper mapper)
        {
            _legalParentRepository = legalParentRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> Create(LegalParentDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (await _legalParentRepository.GetEntities().AnyAsync(x => x.CPF == objectDTO.CPF))
                {
                    responseDTO.SetBadInput("Já existe um responsável legal cadastrado com este CPF!");
                    return responseDTO;
                }

                LegalParent legalParent = _mapper.Map<LegalParent>(objectDTO);

                await _legalParentRepository.InsertAsync(legalParent);
                await _legalParentRepository.SaveChangesAsync();
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
                List<LegalParent> legalParent = await _legalParentRepository.GetEntities().ToListAsync();
                responseDTO.Object = legalParent;
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
                LegalParent? legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (legalParent == null)
                {
                    responseDTO.SetBadInput("Não existe um responsável legal cadastrado com este ID!");
                    return responseDTO;
                }

                _legalParentRepository.Delete(legalParent);
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, LegalParentDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                LegalParent? legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.CPF == objectDTO.CPF);
                if (legalParent == null)
                {
                    responseDTO.SetBadInput("Não existe um responsável legal cadastrado com este CPF!");
                    return responseDTO;
                }

                legalParent = _mapper.Map<LegalParent>(objectDTO);
                await _legalParentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}