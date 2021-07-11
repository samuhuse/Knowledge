
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Models.ViewModel
{
    public class CompanyViewModel
    {
        public Company MyProperty { get; set; }
        public IEnumerable<SelectListItem> Contacts { get; set; }
    }
}
