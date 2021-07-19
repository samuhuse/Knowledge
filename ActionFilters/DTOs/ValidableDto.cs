using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ActionFilters.DTOs
{
    public class ValidableDto : IValidatableObject
    {
        public int? MyNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(MyNumber is null || MyNumber is 0)
            {
                yield return new ValidationResult("My Number can't be null or 0", new[] { MyNumber.ToString()});
            }
        }
    }
}
