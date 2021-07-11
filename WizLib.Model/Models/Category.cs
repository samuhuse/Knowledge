using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizLib.Model.Models
{
    public class Category
    {
        // Id proprety name will make EFCore use it as primary key and configure it as Identity
        public int Id { get; set; } 
        public string Name { get; set; }
    }
}
