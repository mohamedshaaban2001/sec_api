using AutoMapper;
using Entities.Models.Tables;
using StructureGrpc;

namespace GrpcService1.mapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Map from EMPLOYEE (database model) to Employee (Protobuf model)
            //CreateMap<EMPLOYEE, Employee>()
            //    .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EMPLOYEE_ID))
            //    .ForMember(dest => dest.EmpN, opt => opt.MapFrom(src => src.EMP_N))
            //    .ForMember(dest => dest.PosPosC, opt => opt.MapFrom(src => src.POS_POS_C))
            //    .ForMember(dest => dest.EmpC, opt => opt.MapFrom(src => src.EMP_C));

        }
    }
}
