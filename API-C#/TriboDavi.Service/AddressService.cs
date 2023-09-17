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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> Create(AddressDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (await _addressRepository.GetEntities()
                                            .AnyAsync(x => x.Neighborhood == objectDTO.Neighborhood &&
                                                           x.StreetName == objectDTO.StreetName &&
                                                           x.StreetNumber == objectDTO.StreetNumber))
                {
                    responseDTO.SetBadInput("Já existe um endereço cadastrado com esta rua, número e bairro!");
                    return responseDTO;
                }

                Address address = _mapper.Map<Address>(objectDTO);

                address.SetCreatedAt();

                await _addressRepository.InsertAsync(address);
                await _addressRepository.SaveChangesAsync();

                responseDTO.Object = address;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetAddressesForListbox()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _addressRepository.GetEntities()
                                                                .Select(x => new
                                                                {
                                                                    Code = x.Id,
                                                                    Name = $"Rua: {x.StreetName}, Número: {x.StreetNumber}, Bairro: {x.Neighborhood}",
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
                responseDTO.Object = await _addressRepository.GetEntities()
                                                             .Select(x => new
                                                             {
                                                                 x.Id,
                                                                 x.StreetName,
                                                                 x.StreetNumber,
                                                                 x.Neighborhood,
                                                                 x.UpdatedAt,
                                                                 x.CreatedAt,
                                                                 FieldOperationsCount = x.FieldOperations.Count(),
                                                                 StudentsCount = x.Students.Count(),
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
                Address? address = await _addressRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (address == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _addressRepository.Delete(address);
                await _addressRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, AddressDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (await _addressRepository.GetEntities()
                                            .AnyAsync(x => x.Id != id &&
                                                           x.Neighborhood == objectDTO.Neighborhood &&
                                                           x.StreetName == objectDTO.StreetName &&
                                                           x.StreetNumber == objectDTO.StreetNumber))
                {
                    responseDTO.SetBadInput("Já existe um endereço cadastrado com esta rua, número e bairro!");
                    return responseDTO;
                }

                var address = await _addressRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (address == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                PropertyCopier<AddressDTO, Address>.Copy(objectDTO, address);

                address.SetUpdatedAt();

                await _addressRepository.SaveChangesAsync();

                responseDTO.Object = address;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}