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
    public class FieldOperationTeacherService : IFieldOperationTeacherService
    {
        private readonly IFieldOperationTeacherRepository _fieldOperationTeacherRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IFieldOperationRepository _fieldOperationRepository;
        private readonly IMapper _mapper;
        public FieldOperationTeacherService(IFieldOperationTeacherRepository fieldOperationTeacherRepository,
                                            IMapper mapper,
                                            ITeacherRepository teacherRepository,
                                            IFieldOperationRepository fieldOperationRepository)
        {
            _fieldOperationTeacherRepository = fieldOperationTeacherRepository;
            _mapper = mapper;
            _teacherRepository = teacherRepository;
            _fieldOperationRepository = fieldOperationRepository;
        }

        public async Task<ResponseDTO> Create(FieldOperationTeacherDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (!objectDTO.Enabled)
                {
                    responseDTO.SetBadInput("O professor do campo de atuação deve ser ativo!");
                    return responseDTO;
                }

                if (await _fieldOperationTeacherRepository.GetEntities()
                                                          .AnyAsync(x => x.FieldOperation.Id == objectDTO.FieldOperationId &&
                                                                         x.Teacher.Id == objectDTO.TeacherId &&
                                                                         x.Enabled))
                {
                    responseDTO.SetBadInput("Já existe um professor ativo cadastrado neste campo de operação!");
                    return responseDTO;
                }

                var teacher = await _teacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.TeacherId);
                if (teacher == null)
                {
                    responseDTO.SetBadInput("O professor informado não existe!");
                    return responseDTO;
                }

                var fieldOperation = await _fieldOperationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.FieldOperationId);
                if (fieldOperation == null)
                {
                    responseDTO.SetBadInput("O campo de atuação informado não existe!");
                    return responseDTO;
                }

                FieldOperationTeacher fieldOperationTeacher = _mapper.Map<FieldOperationTeacher>(objectDTO);

                fieldOperationTeacher.SetCreatedAt();
                fieldOperationTeacher.Teacher = teacher;
                fieldOperationTeacher.FieldOperation = fieldOperation;

                await _fieldOperationTeacherRepository.InsertAsync(fieldOperationTeacher);
                await _fieldOperationTeacherRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperationTeacher;
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
                responseDTO.Object = await _fieldOperationTeacherRepository.GetEntities()
                                                                           .Include(x => x.Teacher)
                                                                           .Include(x => x.FieldOperation)
                                                                           .Select(x => new
                                                                           {
                                                                               x.Id,
                                                                               TeacherId = x.Teacher.Id,
                                                                               TeacherName = x.Teacher.Name,
                                                                               FieldOperationId = x.FieldOperation.Id,
                                                                               FieldOperationName = x.FieldOperation.Name,
                                                                               x.Enabled,
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

        public async Task<ResponseDTO> GetFieldOperationTeachersForListbox()
        {
            ResponseDTO responseDTO = new();
            try
            {
                var graduation = await _fieldOperationTeacherRepository.GetEntities()
                                                            .Select(x => new
                                                            {
                                                                Code = x.Id,
                                                                Name = $"{x.FieldOperation.Name} - {x.Teacher.Name}",
                                                            }).ToListAsync();
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
                FieldOperationTeacher? fieldOperationTeacher = await _fieldOperationTeacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (fieldOperationTeacher == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _fieldOperationTeacherRepository.Delete(fieldOperationTeacher);
                await _fieldOperationTeacherRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, FieldOperationTeacherDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                FieldOperationTeacher? fieldOperationTeacher = await _fieldOperationTeacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (fieldOperationTeacher == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                if (await _fieldOperationTeacherRepository.GetEntities()
                                                          .AnyAsync(x => x.Id != id &&
                                                                         x.FieldOperation.Id == objectDTO.FieldOperationId &&
                                                                         x.Teacher.Id == objectDTO.TeacherId &&
                                                                         x.Enabled))
                {
                    responseDTO.SetBadInput("Já existe um professor ativo cadastrado neste campo de operação!");
                    return responseDTO;
                }

                var teacher = await _teacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.TeacherId);
                if (teacher == null)
                {
                    responseDTO.SetBadInput("O professor informado não existe!");
                    return responseDTO;
                }

                var fieldOperation = await _fieldOperationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.FieldOperationId);
                if (fieldOperation == null)
                {
                    responseDTO.SetBadInput("O campo de atuação informado não existe!");
                    return responseDTO;
                }

                PropertyCopier<FieldOperationTeacherDTO, FieldOperationTeacher>.Copy(objectDTO, fieldOperationTeacher);

                fieldOperationTeacher.SetUpdatedAt();
                fieldOperationTeacher.Teacher = teacher;
                fieldOperationTeacher.FieldOperation = fieldOperation;

                await _fieldOperationTeacherRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperationTeacher;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}