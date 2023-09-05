using AutoMapper;
using TriboDavi.Domain;
using TriboDavi.Domain.Identity;
using TriboDavi.DTO;

namespace TriboDavi.Service
{
    public class TriboDaviProfile : Profile
    {
        public TriboDaviProfile()
        {
            CreateMap<User, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<User, UserLoginDTO>(MemberList.None).ReverseMap();
            CreateMap<LegalParent, LegalParentDTO>(MemberList.None).ReverseMap();
            CreateMap<Student, StudentDTO>(MemberList.None).ReverseMap();
            CreateMap<Address, AddressDTO>(MemberList.None).ReverseMap();
            CreateMap<Graduation, GraduationDTO>(MemberList.None).ReverseMap();
            CreateMap<Teacher, TeacherDTO>(MemberList.None).ReverseMap();
            CreateMap<FieldOperation, FieldOperationDTO>(MemberList.None).ReverseMap();
        }
    }
}