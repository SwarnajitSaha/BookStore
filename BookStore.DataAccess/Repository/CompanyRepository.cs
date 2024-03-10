using BookStore.Data;
using BookStore.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDBContext _db;

        public CompanyRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }

        public void update(Company company)
        {
            _db.CompanyTable.Update(company);
        }
    }
}
