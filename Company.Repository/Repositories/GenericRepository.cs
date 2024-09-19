using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Company.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity

    {
        private readonly CompanyDbContext _context;
        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        => _context.Set<T>().Add(entity);

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
          => _context.Set<T>().ToList();
        
        public T GetById(int? id)
          => _context.Set<T>().AsNoTracking().FirstOrDefault(x=>x.Id == id);

        public void Update(T entity)
        => _context.Set<T>().Update(entity);
    }

}
