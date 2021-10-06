using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Results;
using NUnit.Framework;

namespace BehavioralPatterns.Validations
{
    public class FluentValidations
    {
        #region Model
        public class Address
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Town { get; set; }
            public string County { get; set; }
            public string Postcode { get; set; }
        }
        public class Customer
        {
            public string Name { get; set; }
            public Address Address { get; set; }

            public List<string> Products { get; set; } = new List<string>();
        }        
        public class CustomerPurchaseSaga
        {
            public Customer Customer { get; set; }
            public List<Order> Orders { get; set; }

            public class Order
            {
                public string ProductName { get; set; }
                public int ItemCount { get; set; }
                public double Total { get; set; }
                public DateTime PurchaseDate { get; set; }
            }
        }

        // Ineritance

        public interface IContact
        {
            string Name { get; set; }
            string Email { get; set; }
        }
        public class Person : IContact
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
        public class Organisation : IContact
        {
            public string Name { get; set; }
            public string Email { get; set; }

            public Address HeadQuarters { get; set; }
        }
        public class ContactRequest
        {
            public IContact Contact { get; set; }
            public string MessageToSend { get; set; }
        }

        // Conditions
        public class User
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Nickname { get; set; }
        }

        #endregion

        #region Validator

        public class AddressValidator : AbstractValidator<Address>
        {
            public AddressValidator()
            {
                RuleFor(address => address.Postcode).NotNull().NotEmpty();
                RuleFor(address => address.Town).NotNull();
                RuleFor(address => address.County).NotNull().NotEmpty().WithMessage("Must specify County"); // Custom error message
            }
        }
        public class CustomerValidator : AbstractValidator<Customer>
        {
            public CustomerValidator()
            {
                
                RuleFor(customer => customer.Name).NotNull();
                RuleFor(customer => customer.Address).SetValidator(new AddressValidator());

                RuleFor(customer => customer.Address.Line1).NotNull().NotEmpty(); // Inner class

                RuleForEach(c => c.Products).NotEmpty(); // IEnumerbles
            }
        }

        public class CustomerPurchaseSagaCountValidator : AbstractValidator<CustomerPurchaseSaga>
        {
            public CustomerPurchaseSagaCountValidator()
            {
                RuleFor(s => s.Orders).Must(o => o.Count < 100).WithMessage("Custom error");
            }
        }
        public class CustomerPurchaseSagaValidator : AbstractValidator<CustomerPurchaseSaga>
        {
            public CustomerPurchaseSagaValidator()
            {
                RuleForEach(s => s.Orders).ChildRules(orders =>
                {
                    orders.RuleFor(o => o.Total).GreaterThan(0);
                    orders.RuleFor(o => o.ProductName).NotNull().NotEmpty();
                    orders.RuleFor(o => o.ItemCount).GreaterThan(0).LessThan(100);
                    orders.RuleFor(o => o.ProductName).NotEmpty().NotNull();
                });

                Include(new CustomerPurchaseSagaCountValidator());

                RuleFor(s => s.Customer).NotNull().When(c => c is not null).SetValidator(new CustomerValidator());
            }
        }

        // Ineritanche
        public class PersonValidator : AbstractValidator<Person>
        {
            public PersonValidator()
            {
                RuleFor(x => x.Name).NotNull();
                RuleFor(x => x.Email).NotNull();
                RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.MinValue);
            }
        }
        public class OrganisationValidator : AbstractValidator<Organisation>
        {
            public OrganisationValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty().WithName("Contact Mail").WithMessage("Invalid organization {PropertyName}");
                RuleFor(x => x.HeadQuarters).SetValidator(new AddressValidator());
            }
        }
        public class IContactValidator : AbstractValidator<ContactRequest>
        {
            public IContactValidator()
            {
                RuleFor(c => c.Contact.Email).Cascade(CascadeMode.Stop).NotNull().NotEmpty().Must(e => e.Contains("@"))
                .DependentRules(() => {
                    RuleFor(x => x.Contact).SetInheritanceValidator(v =>
                    {
                        v.Add<Organisation>((o) => new OrganisationValidator()); // Lazy construction
                        v.Add<Person>(new PersonValidator());
                    });
                    });
            }
        }

        // Result sets
        public class CustomerValidatorRuleSets : AbstractValidator<Customer>
        {
            public CustomerValidatorRuleSets()
            {
                RuleSet("Products", () =>
                {
                    RuleForEach(c => c.Products).NotEmpty().WithName("Product name");
                });

                RuleSet("Address", () =>
                {
                    RuleFor(c => c.Address).SetValidator(new AddressValidator());
                });

                RuleFor(customer => customer.Name).NotNull();
            }
        }

        // Conditions
        public class UserValidator : AbstractValidator<User>
        {
            public UserValidator()
            {
                RuleFor(u => u.Name).NotEmpty().NotNull().When(c => string.IsNullOrEmpty(c.Nickname));
                RuleFor(u => u.Surname).NotEmpty().NotNull().When(c => string.IsNullOrEmpty(c.Nickname));
                RuleFor(u => u.Nickname).NotEmpty().NotNull().When(c => string.IsNullOrEmpty(c.Name) && string.IsNullOrEmpty(c.Surname));
            }
        }

        // SeverityLevel
        public class UserSeverityValidator : AbstractValidator<User>
        {       
            public UserSeverityValidator()
            {
                RuleFor(u => u.Name).Must((name) => !string.IsNullOrEmpty(name)).WithSeverity(u => 
                {
                    return string.IsNullOrEmpty(u.Nickname) ? Severity.Error : Severity.Warning;
                });

                RuleFor(u => u.Surname).Must((surname) => !string.IsNullOrEmpty(surname)).WithSeverity(u =>
                {
                    return string.IsNullOrEmpty(u.Nickname) ? Severity.Error : Severity.Warning;
                });

                RuleFor(u => u.Nickname).Must((nickName) => !string.IsNullOrEmpty(nickName)).WithSeverity(u =>
                {
                    return string.IsNullOrEmpty(u.Name) && string.IsNullOrEmpty(u.Surname) ? Severity.Error : Severity.Warning;
                });
            }
        }

        // Error codes
        public class UserErrorCodesValidator : AbstractValidator<User>
        {

            public UserErrorCodesValidator()
            {
                RuleFor(u => u.Name).Must((name) => !string.IsNullOrEmpty(name)).WithErrorCode("MissingName");

                RuleFor(u => u.Surname).Must((surname) => !string.IsNullOrEmpty(surname)).WithErrorCode("MissingSurname");

                RuleFor(u => u.Nickname).Must((nickName) => !string.IsNullOrEmpty(nickName)).WithErrorCode("MissingNickname");
            }
        }

        #endregion


        [Test]
        public void Try()
        {            
            #region Test istances

            Customer validCustomer = new Customer
            {
                Name = "Samuele",
                Products = new List<string> { "Laptop" },
                Address = new Address
                {
                    Line1 = "via XXXX",
                    County = "Italy",
                    Town = "Monza",
                    Postcode = "20900"
                }
            };
            Customer invalidCustomer = new Customer
            {
                Name = null,
                Products = new List<string> {""},
                Address = new Address
                {
                    County = string.Empty,
                    Town = null,
                    Postcode = "20900"
                }
            };

            CustomerPurchaseSaga validSaga = new CustomerPurchaseSaga
            {
                Customer = new Customer
                {
                    Name = "Samuele",
                    Products = new List<string> { "Laptop" },
                    Address = new Address
                    {
                        Line1 = "via XXXX",
                        County = "Italy",
                        Town = "Monza",
                        Postcode = "20900"
                    }
                },
                Orders = new List<CustomerPurchaseSaga.Order>
                {
                    new CustomerPurchaseSaga.Order
                    {
                        Total = 100,
                        ProductName = "Laptop",
                        ItemCount = 1,
                        PurchaseDate = DateTime.Now
                    }
                }
            };
            CustomerPurchaseSaga invalidSaga = new CustomerPurchaseSaga
            {
                Customer = new Customer
                {
                    Name = "Samuele",
                    Products = new List<string> { "" },
                    Address = new Address
                    {
                        County = string.Empty,
                        Town = null,
                        Postcode = "20900"
                    }
                },
                Orders = new List<CustomerPurchaseSaga.Order>
                {
                    new CustomerPurchaseSaga.Order
                    {
                        Total = -1,
                        ProductName = "Laptop",
                        ItemCount = 101,
                        PurchaseDate = DateTime.Now
                    }
                }
            };

            ContactRequest validRequest = new ContactRequest
            {
                Contact = new Person
                {
                    Name = "Samuele",
                    Email = "samuele.mn@hotmail.it",
                    DateOfBirth = DateTime.Now
                },
                MessageToSend = "Hi to every one"
            };
            ContactRequest invalidRequest = new ContactRequest
            {
                Contact = new Organisation
                {
                    Name = "",
                    Email = ""
                },
                MessageToSend = "Hi to every one"
            };

            User validUser = new User
            {
                Name = "",
                Surname = "",
                Nickname = "Samuhuse"
            };
            User invalidUser = new User
            {
                Name = "Samuele",
                Surname = "",
                Nickname = ""
            };

            #endregion

            PrintResult(nameof(validCustomer), new CustomerValidator().Validate(validCustomer));
            PrintResult(nameof(invalidCustomer), new CustomerValidator().Validate(invalidCustomer));

            new CustomerValidator().ValidateAndThrow(validCustomer); // Thrown an exception if is invalid

            PrintResult(nameof(validSaga), new CustomerPurchaseSagaValidator().Validate(validSaga));
            PrintResult(nameof(invalidSaga), new CustomerPurchaseSagaValidator().Validate(invalidSaga));

            PrintResult(nameof(validRequest), new IContactValidator().Validate(validRequest));
            PrintResult(nameof(invalidRequest), new IContactValidator().Validate(invalidRequest));

            PrintResult("Rule set " + nameof(validCustomer), new CustomerValidatorRuleSets().Validate(validCustomer));
            PrintResult("Rule set without Address " + nameof(invalidCustomer), new CustomerValidatorRuleSets().Validate(invalidCustomer, options => options.IncludeRuleSets("Products","default")));
            PrintResult("Rule set with Address " + nameof(invalidCustomer), new CustomerValidatorRuleSets().Validate(invalidCustomer, options => options.IncludeRuleSets("Products","Address", "default")));

            PrintResult(nameof(validUser), new UserValidator().Validate(validUser));
            PrintResult(nameof(invalidUser), new UserValidator().Validate(invalidUser));

            PrintResult(nameof(validUser), new UserSeverityValidator().Validate(validUser));
            PrintResult(nameof(invalidUser), new UserSeverityValidator().Validate(invalidUser));

            PrintResult(nameof(validUser), new UserErrorCodesValidator().Validate(validUser));
            PrintResult(nameof(invalidUser), new UserErrorCodesValidator().Validate(invalidUser));

        }

        private void PrintResult(string objectName ,ValidationResult result)
        {
            Console.WriteLine($"Result state of {objectName}: { result.IsValid}");

            if (result.Errors.Count == 0) Console.WriteLine("no errors");
            else result.Errors.ForEach(e => Console.WriteLine($"error: {e}, Severity: {e.Severity}, ErrorCode {e.ErrorCode}"));

            Console.WriteLine();
        }
    }
}
