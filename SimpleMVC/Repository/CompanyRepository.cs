using SimpleMvcConsumer.Models;
using SimpleMvcConsumer.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}
