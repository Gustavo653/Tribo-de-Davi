using AutoMapper;
using Common.DTO;
using Common.Functions;
using Microsoft.EntityFrameworkCore;
using TriboDavi.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.Service
{
    public class FieldOperationService : IFieldOperationService
    {
        private readonly IFieldOperationRepository _fieldOperationRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public FieldOperationService(IFieldOperationRepository fieldOperationRepository, IMapper mapper, IAddressRepository addressRepository)
        {
            _fieldOperationRepository = fieldOperationRepository;
            _mapper = mapper;
            _addressRepository = addressRepository;
        }

        public async Task<ResponseDTO> Create(FieldOperationDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (await _fieldOperationRepository.GetEntities().AnyAsync(x => x.Name == objectDTO.Name))
                {
                    responseDTO.SetBadInput("Já existe um campo de operação cadastrado com este nome!");
                    return responseDTO;
                }

                var address = await _addressRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.AddressId);
                if (address == null)
                {
                    responseDTO.SetBadInput("O endereço informado não existe!");
                    return responseDTO;
                }

                FieldOperation fieldOperation = _mapper.Map<FieldOperation>(objectDTO);
                fieldOperation.Address = address;
                fieldOperation.SetCreatedAt();

                await _fieldOperationRepository.InsertAsync(fieldOperation);
                await _fieldOperationRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperation;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetFieldOperationsForListbox()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _fieldOperationRepository.GetEntities()
                                                                .Select(x => new
                                                                {
                                                                    Code = x.Id,
                                                                    Name = x.Name,
                                                                }).ToListAsync();
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
                responseDTO.Object = await _fieldOperationRepository.GetEntities()
                                                                    .Select(x => new
                                                                    {
                                                                        x.Id,
                                                                        x.Name,
                                                                        AddressId = x.Address.Id,
                                                                        x.CreatedAt,
                                                                        x.UpdatedAt
                                                                    })
                                                                    .ToListAsync();
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
                FieldOperation? fieldOperation = await _fieldOperationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (fieldOperation == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _fieldOperationRepository.Delete(fieldOperation);
                await _fieldOperationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, FieldOperationDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                FieldOperation? fieldOperation = await _fieldOperationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (fieldOperation == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                if (await _fieldOperationRepository.GetEntities().AnyAsync(x => x.Id != id && (x.Name == objectDTO.Name)))
                {
                    responseDTO.SetBadInput("Já existe um campo de operação cadastrado com este nome!");
                    return responseDTO;
                }

                var address = await _addressRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.AddressId);
                if (address == null)
                {
                    responseDTO.SetBadInput("O endereço informado não existe!");
                    return responseDTO;
                }

                PropertyCopier<FieldOperationDTO, FieldOperation>.Copy(objectDTO, fieldOperation);
                fieldOperation.Address = address;
                fieldOperation.SetUpdatedAt();

                await _fieldOperationRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperation;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}