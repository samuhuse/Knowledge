using Microsoft.EntityFrameworkCore;

using SimpleApi.Data;
using SimpleApi.Model;
using SimpleApi.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleApi.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override IEnumerable<Contact> GetAll()
        {
            return Context.Set<Contact>().Include(c => c.Company);
        }

        public override Contact Get(int id)
        {
            return Context.Set<Contact>().Include(c => c.Company).SingleOrDefault(c => c.Id == id);
        }

        public override Contact SingleOrDefault(Expression<Func<Contact, bool>> predicate)
        {
            return Context.Set<Contact>().Include(c => c.Company).SingleOrDefault(predicate);
        }

        public IEnumerable<Contact> GetByCompanyId(int id)
        {
            return Context.Set<Contact>().Where(c => c.CompanyId == id).Include(c => c.Company);
        }
    }
}
