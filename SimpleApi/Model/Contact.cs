using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Model
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string? Email { get; set; }
        public DateTime InsertDate { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public static class ContactFactory
        {
            public static Contact Create(int id,string name, string surname, Company company)
            {
                return new Contact() { Id = id, Name = name, Surname = surname, Email = $"{name}@{surname}.com", CompanyId = company.Id };
            }
        }
    }
}
