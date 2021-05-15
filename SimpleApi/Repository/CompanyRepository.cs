using Microsoft.EntityFrameworkCore;

using SimpleApi.Data;
using SimpleApi.Model;
using SimpleApi.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public override IEnumerable<Company> GetAll()
        {
            return Context.Set<Company>().Include(c => c.Contacts);
        }
    }
}
