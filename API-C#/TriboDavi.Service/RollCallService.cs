using Common.DTO;
using Microsoft.EntityFrameworkCore;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Service.Interface;

namespace TriboDavi.Service
{
    public class RollCallService : IRollCallService
    {
        private readonly IFieldOperationStudentRepository _fieldOperationStudentRepository;
        private readonly IRollCallRepository _rollCallRepository;

        public RollCallService(IFieldOperationStudentRepository fieldOperationStudentRepository, IRollCallRepository rollCallRepository)
        {
            _fieldOperationStudentRepository = fieldOperationStudentRepository;
            _rollCallRepository = rollCallRepository;
        }

        public async Task<ResponseDTO> GenerateRollCall()
        {
            ResponseDTO responseDTO = new();
            try
            {
                var fieldOperationTeachers = await _fieldOperationStudentRepository.GetTrackedEntities()
                                                                                   .Include(x => x.FieldOperationTeacher)
                                                                                   .Include(x => x.Student)
                                                                                   .Where(x => x.Enabled)
                                                                                   .ToListAsync();
                var rollCalls = new List<RollCall>();
                foreach (var item in fieldOperationTeachers)
                {
                    RollCall rollCall = new() { Date = DateTime.Now, FieldOperationStudent = item, Presence = false };
                    rollCall.SetCreatedAt();
                    rollCalls.Add(rollCall);
                }
                _rollCallRepository.InsertRange(rollCalls);
                await _rollCallRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}