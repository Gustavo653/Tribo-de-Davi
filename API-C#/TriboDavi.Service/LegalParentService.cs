using AutoMapper;
using Common.DTO;
using Common.Functions;
using Microsoft.EntityFrameworkCore;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

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

                legalParent.SetCreatedAt();

                await _legalParentRepository.InsertAsync(legalParent);
                await _legalParentRepository.SaveChangesAsync();

                responseDTO.Object = legalParent;
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
                responseDTO.Object = await _legalParentRepository.GetEntities()
                                                                 .Include(x => x.Students)
                                                                 .Select(x => new
                                                                 {
                                                                     x.Id,
                                                                     x.Name,
                                                                     x.Relationship,
                                                                     x.RG,
                                                                     x.CPF,
                                                                     x.PhoneNumber,
                                                                     x.CreatedAt,
                                                                     x.UpdatedAt,
                                                                     StudentsCount = x.Students.Count
                                                                 }).ToListAsync();
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
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _legalParentRepository.Delete(legalParent);
                await _legalParentRepository.SaveChangesAsync();
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
                LegalParent? legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (legalParent == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                PropertyCopier<LegalParentDTO, LegalParent>.Copy(objectDTO, legalParent);

                legalParent.SetUpdatedAt();

                await _legalParentRepository.SaveChangesAsync();

                responseDTO.Object = legalParent;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}