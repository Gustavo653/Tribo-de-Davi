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
    public class GraduationService : IGraduationService
    {
        private readonly IGraduationRepository _graduationRepository;
        private readonly IMapper _mapper;
        private readonly IGoogleCloudStorageService _googleCloudStorageService;
        public GraduationService(IGraduationRepository graduationRepository, IMapper mapper, IGoogleCloudStorageService googleCloudStorageService)
        {
            _graduationRepository = graduationRepository;
            _mapper = mapper;
            _googleCloudStorageService = googleCloudStorageService;
        }

        public async Task<ResponseDTO> Create(GraduationDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (await _graduationRepository.GetEntities().AnyAsync(x => x.Name == objectDTO.Name || x.Position == objectDTO.Position))
                {
                    responseDTO.SetBadInput("Já existe uma graduação cadastrada com este nome e/ou posição!");
                    return responseDTO;
                }

                Graduation graduation = _mapper.Map<Graduation>(objectDTO);

                graduation.Url = await _googleCloudStorageService.UploadFileToGcsAsync(objectDTO.File, $"{Guid.NewGuid()}{Path.GetExtension(objectDTO.File.FileName)}");
                graduation.SetCreatedAt();

                await _graduationRepository.InsertAsync(graduation);
                await _graduationRepository.SaveChangesAsync();

                responseDTO.Object = graduation;
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
                List<Graduation> graduation = await _graduationRepository.GetEntities().ToListAsync();
                responseDTO.Object = graduation;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetGraduationsForListbox()
        {
            ResponseDTO responseDTO = new();
            try
            {
                var graduation = await _graduationRepository.GetEntities()
                                                            .OrderBy(x => x.Position)
                                                            .Select(x => new
                                                            {
                                                                Code = x.Id,
                                                                Name = x.Name,
                                                            })
                                                            .ToListAsync();
                responseDTO.Object = graduation;
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
                Graduation? graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (graduation == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                await _googleCloudStorageService.DeleteFileFromGcsAsync(graduation.Url);
                _graduationRepository.Delete(graduation);
                await _graduationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, GraduationDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                Graduation? graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (graduation == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                if (await _graduationRepository.GetEntities().AnyAsync(x => x.Id != id && (x.Name == objectDTO.Name || x.Position == objectDTO.Position)))
                {
                    responseDTO.SetBadInput("Já existe uma graduação cadastrada com este nome e/ou posição!");
                    return responseDTO;
                }

                PropertyCopier<GraduationDTO, Graduation>.Copy(objectDTO, graduation);

                await _googleCloudStorageService.DeleteFileFromGcsAsync(graduation.Url);
                graduation.SetUpdatedAt();
                graduation.Url = await _googleCloudStorageService.UploadFileToGcsAsync(objectDTO.File, $"{Guid.NewGuid()}{Path.GetExtension(objectDTO.File.FileName)}");

                await _graduationRepository.SaveChangesAsync();

                responseDTO.Object = graduation;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}