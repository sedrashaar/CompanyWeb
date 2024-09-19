using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Repository.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext Context) : base(Context)
        {
            _context = Context;
        }
  
     
        public IEnumerable<Employee> GetEmployeesByName(string name)
          => _context.Employees.Where(x => x.Name.Trim().ToLower().Contains(name.Trim().ToLower()) ||
           x.Email.Trim().ToLower().Contains(name.Trim().ToLower()) ||
           x.PhoneNumber.Trim().ToLower().Contains(name.Trim().ToLower())

            ).ToList();
    }
}
