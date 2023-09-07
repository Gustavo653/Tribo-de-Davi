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
    public class FieldOperationStudentService : IFieldOperationStudentService
    {
        private readonly IFieldOperationStudentRepository _fieldOperationStudentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IFieldOperationTeacherRepository _fieldOperationTeacherRepository;
        private readonly IMapper _mapper;
        public FieldOperationStudentService(IMapper mapper,
                                            IFieldOperationTeacherRepository fieldOperationTeacherRepository,
                                            IStudentRepository studentRepository,
                                            IFieldOperationStudentRepository fieldOperationStudentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _fieldOperationStudentRepository = fieldOperationStudentRepository;
            _fieldOperationTeacherRepository = fieldOperationTeacherRepository;
        }

        public async Task<ResponseDTO> Create(FieldOperationStudentDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (!objectDTO.Enabled)
                {
                    responseDTO.SetBadInput("O aluno do campo de atuação deve ser ativo!");
                    return responseDTO;
                }

                if (await _fieldOperationStudentRepository.GetEntities()
                                                          .AnyAsync(x => x.FieldOperationTeacher.Id == objectDTO.FieldOperationTeacherId &&
                                                                         x.Student.Id == objectDTO.StudentId &&
                                                                         x.Enabled))
                {
                    responseDTO.SetBadInput("Já existe um aluno ativo cadastrado neste campo de operação!");
                    return responseDTO;
                }

                var student = await _studentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.StudentId);
                if (student == null)
                {
                    responseDTO.SetBadInput("O aluno informado não existe!");
                    return responseDTO;
                }

                var fieldOperationTeacher = await _fieldOperationTeacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.FieldOperationTeacherId);
                if (fieldOperationTeacher == null)
                {
                    responseDTO.SetBadInput("O campo de atuação informado não existe!");
                    return responseDTO;
                }

                FieldOperationStudent fieldOperationStudent = _mapper.Map<FieldOperationStudent>(objectDTO);

                fieldOperationStudent.SetCreatedAt();
                fieldOperationStudent.Student = student;
                fieldOperationStudent.FieldOperationTeacher = fieldOperationTeacher;

                await _fieldOperationStudentRepository.InsertAsync(fieldOperationStudent);
                await _fieldOperationStudentRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperationStudent;
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
                responseDTO.Object = await _fieldOperationStudentRepository.GetEntities()
                                                                           .Include(x => x.Student)
                                                                           .Include(x => x.FieldOperationTeacher).ThenInclude(x => x.Teacher)
                                                                           .Include(x => x.FieldOperationTeacher).ThenInclude(x => x.FieldOperation)
                                                                           .Select(x => new
                                                                           {
                                                                               x.Id,
                                                                               StudentId = x.Student.Id,
                                                                               StudentName = x.Student.Name,
                                                                               FieldOperationTeacherId = x.FieldOperationTeacher.Id,
                                                                               FieldOperationTeacherName = $"{x.FieldOperationTeacher.FieldOperation.Name} - {x.FieldOperationTeacher.Teacher.Name}",
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

        public async Task<ResponseDTO> Remove(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                FieldOperationStudent? fieldOperationStudent = await _fieldOperationStudentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (fieldOperationStudent == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _fieldOperationStudentRepository.Delete(fieldOperationStudent);
                await _fieldOperationStudentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, FieldOperationStudentDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                FieldOperationStudent? fieldOperationStudent = await _fieldOperationStudentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (fieldOperationStudent == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                if (await _fieldOperationStudentRepository.GetEntities()
                                           .AnyAsync(x => x.Id != id &&
                                                          x.FieldOperationTeacher.Id == objectDTO.FieldOperationTeacherId &&
                                                          x.Student.Id == objectDTO.StudentId &&
                                                          x.Enabled))
                {
                    responseDTO.SetBadInput("Já existe um aluno ativo cadastrado neste campo de operação!");
                    return responseDTO;
                }

                var student = await _studentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.StudentId);
                if (student == null)
                {
                    responseDTO.SetBadInput("O aluno informado não existe!");
                    return responseDTO;
                }

                var fieldOperationTeacher = await _fieldOperationTeacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.FieldOperationTeacherId);
                if (fieldOperationTeacher == null)
                {
                    responseDTO.SetBadInput("O campo de atuação informado não existe!");
                    return responseDTO;
                }

                PropertyCopier<FieldOperationStudentDTO, FieldOperationStudent>.Copy(objectDTO, fieldOperationStudent);

                fieldOperationStudent.SetUpdatedAt();
                fieldOperationStudent.Student = student;
                fieldOperationStudent.FieldOperationTeacher = fieldOperationTeacher;

                await _fieldOperationStudentRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperationStudent;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}