using AutoMapper;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Services.Interfaces;
using Company.Services.Interfaces.Department.Dto;
using Company.Services.Interfaces.Employee.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public void Add(DepartmentDto DepartmentDto)
        {
            //var mappedDepartment = new DepartmentDto 
            //{
            //    Code = DepartmentDto.Code,
            //    Name = DepartmentDto.Name,
            //    CreateAt = DateTime.Now,
            //}:
            var mappedDepartment = _mapper.Map<Department>(DepartmentDto);
            _unitOfWork.DepartmentRepository.Add(mappedDepartment);
            _unitOfWork.Complete();
        }

        public void Delete(DepartmentDto DepartmentDto)
        {
            var mappedDepartment = _mapper.Map<Department>(DepartmentDto);
            _unitOfWork.DepartmentRepository.Delete(mappedDepartment);
            _unitOfWork.Complete();
        }

        public IEnumerable<DepartmentDto> GetAll()
        {
            var dapartments = _unitOfWork.DepartmentRepository.GetAll();
            var mappedDepartments = _mapper.Map<IEnumerable<DepartmentDto>>(dapartments);
            return mappedDepartments;
        }

        public DepartmentDto GetById(int? id)
        {
            if (id is null)
                return null;
            var department= _unitOfWork.DepartmentRepository.GetById(id.Value);
            if (department is null)
                 return null;

            var  mappedDepartment = _mapper.Map<DepartmentDto>(department);

            return mappedDepartment;
        }

        public void Update(DepartmentDto DepartmentDto)
        {
            //Department department = _mapper.Map<Department>(DepartmentDto);
            //_unitOfWork.DepartmentRepository.Update(department);
            //_unitOfWork.Complete();
        }
    }
}
