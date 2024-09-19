using AutoMapper;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Services.Helper;
using Company.Services.Interfaces;
using Company.Services.Interfaces.Employee.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(EmployeeDto employeeDto)
        {
            //Manual mapping
            //Employee employee = new Employee
            //{
            //    Name = EmployeeDto.Name,
            //    Age = EmployeeDto.Age,
            //    DepartmentId = EmployeeDto.DepartmentId,
            //    Address = EmployeeDto.Address,
            //    Salary = EmployeeDto.Salary,
            //    Email = EmployeeDto.Email,
            //    PhoneNumber = EmployeeDto.PhoneNumber,
            //    HiringDate = EmployeeDto.HiringDate,
            //    ImageUrl = EmployeeDto.ImageUrl
            //};

            employeeDto.ImageUrl = DocumentSettings.UploadFile(employeeDto.Image, "Images");
            Employee employee = _mapper.Map<Employee>( employeeDto );
            _unitOfWork.EmployeeRepository.Add(employee);
            _unitOfWork.Complete();
        }

        public void Delete(EmployeeDto EmployeeDto)
        {
            //Employee employee = new Employee
            //{
            //    Name = EmployeeDto.Name,
            //    Age = EmployeeDto.Age,
            //    DepartmentId = EmployeeDto.DepartmentId,
            //    Address = EmployeeDto.Address,
            //    Salary = EmployeeDto.Salary,
            //    Email = EmployeeDto.Email,
            //    PhoneNumber = EmployeeDto.PhoneNumber,
            //    HiringDate = EmployeeDto.HiringDate,
            //    ImageUrl = EmployeeDto.ImageUrl
            //};

            Employee employee = _mapper.Map<Employee>(EmployeeDto);

            _unitOfWork.EmployeeRepository.Delete(employee);
            _unitOfWork.Complete();
        }

        public IEnumerable<EmployeeDto> GetAll()
        {
            var employees = _unitOfWork.EmployeeRepository.GetAll();
            //var mappedEmployees = employees.Select(x => new EmployeeDto
            //{
            //    DepartmentId=x.DepartmentId,
            //    Address=x.Address,
            //    Salary=x.Salary,
            //    Email=x.Email,
            //    PhoneNumber=x.PhoneNumber,
            //    HiringDate=x.HiringDate,
            //    ImageUrl=x.ImageUrl,
            //    Id=x.Id,
            //    Age=x.Age,
            //    CreateAt=x.CreateAt,
            //    Name=x.Name
            //});
            IEnumerable<EmployeeDto> mappedEmployees = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return mappedEmployees;
        }
        public void Update(EmployeeDto EmployeeDto)
        {
            //Employee employee = new Employee
            //{
            //    Name = EmployeeDto.Name,
            //    Age = EmployeeDto.Age,
            //    DepartmentId = EmployeeDto.DepartmentId,
            //    Address = EmployeeDto.Address,
            //    Salary = EmployeeDto.Salary,
            //    Email = EmployeeDto.Email,
            //    PhoneNumber = EmployeeDto.PhoneNumber,
            //    HiringDate = EmployeeDto.HiringDate,
            //    ImageUrl = EmployeeDto.ImageUrl
            //};

            //Employee employee = _mapper.Map<Employee>(EmployeeDto);
            //_unitOfWork.EmployeeRepository.Update(employee);
            //_unitOfWork.Complete();
        }
        public IEnumerable<EmployeeDto> GetEmployeesByName(string name)
        {
            var employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(name);
            //var mappedEmployees = employees.Select(x => new EmployeeDto
            //{
            //    DepartmentId = x.DepartmentId,
            //    Address = x.Address,
            //    Salary = x.Salary,
            //    Email = x.Email,
            //    PhoneNumber = x.PhoneNumber,
            //    HiringDate = x.HiringDate,
            //    ImageUrl = x.ImageUrl,
            //    Id = x.Id,
            //    Age = x.Age,
            //    CreateAt = x.CreateAt,
            //    Name = x.Name
            //});
            IEnumerable<EmployeeDto> mappedEmployees = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return mappedEmployees;
        }

        public EmployeeDto GetById(int? id)
        {
            if (id is null)
                return null;
            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            if (employee is null)
                return null;
            //EmployeeDto employeeDto = new EmployeeDto
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    DepartmentId = employee.DepartmentId,
            //    Address = employee.Address,
            //    Salary = employee.Salary,
            //    Email = employee.Email,
            //    PhoneNumber = employee.PhoneNumber,
            //    HiringDate = employee.HiringDate,
            //    ImageUrl = employee.ImageUrl,
            //    Id = employee.Id,
            //    CreateAt= employee.CreateAt

            //};
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }
    }
}

