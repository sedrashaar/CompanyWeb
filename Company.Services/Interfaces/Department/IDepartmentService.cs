using Company.Data.Entities;
using Company.Services.Interfaces.Department.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Interfaces
{
    public interface IDepartmentService
    {
        DepartmentDto GetById(int? id);
        IEnumerable<DepartmentDto> GetAll();
        void Add(DepartmentDto departmentDto);
        void Update(DepartmentDto departmentDto);
        void Delete(DepartmentDto departmentDto);
    }
}
