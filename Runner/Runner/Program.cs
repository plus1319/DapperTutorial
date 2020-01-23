using DataLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Runner
{
    class Program
    {
        private static IConfigurationRoot config;

        static void Main(string[] args)
        {
            Initialize();
            //Get_all_should_result_6_results();
            //Insert_should_assign_identity_to_new_entity();
            //Find_should_retrieve_existing_entity(1);
            //Modify_should_update_existing_entity(1);
            //Delete_should_remove_entity(16);
            GetComplex_should_get_entities(1);
            Console.ReadLine();
        }

        static void GetComplex_should_get_entities(int id)
        {
            //arrange
            IContactRepository repository = CreateRepository();

            //act
            var contact = repository.GetFullContact(id);

            //assert
            Console.WriteLine($"contact firstName:{contact.FirstName}");
            foreach (var contactAddress in contact.Addresses)
            {
                Console.WriteLine("-----------------------");
                Console.WriteLine($"address is:{contactAddress.StreetAddress}");
            }
        }
        static void Delete_should_remove_entity(int id)
        {
            //arrange 
            IContactRepository repository = CreateRepository();

            //act
            repository.Remove(id);

            //create a new repository to verification  purposes
            IContactRepository repository1 = CreateRepository();
            var deletedEntity = repository1.Find(id);
            //assert
            Console.WriteLine($"is Deleted entity :{deletedEntity == null}");
            Console.WriteLine("*** Contact Deleted ***");
        }

        static void Modify_should_update_existing_entity(int id)
        {
            //arrange 
            IContactRepository repository = CreateRepository();

            //act
            var contact = repository.Find(id);
            contact.FirstName = "Bob";
            repository.Update(contact);

            //create a new repository for verification purposes
            IContactRepository repository1 = CreateRepository();
            var modifiedContact = repository1.Find(id);

            //assert
            Console.WriteLine("*** Contact Modified ***");
            Console.WriteLine($"is modified name is Bob ?:{modifiedContact.FirstName == "Bob"}");
        }
        static void Find_should_retrieve_existing_entity(int id)
        {
            //arrange
            IContactRepository repository = CreateRepository();

            //act 
            var contact = repository.Find(id);

            //assert
            Console.WriteLine($"contact name is:{contact.FirstName}");
        }

        static int Insert_should_assign_identity_to_new_entity()
        {
            //arrange
            IContactRepository repository = CreateRepository();
            var contact = new Contact
            {
                FirstName = "Joe",
                LastName = "Blow",
                Email = "joe.blow@gmail.com",
                Company = "Microsoft",
                Title = "Developer"
            };

            //act 
            repository.Add(contact);

            //assert
            Console.WriteLine($"Contact inserted is :{contact.Id != 0}"); 
            Console.WriteLine($"New Id : {contact.Id}");
            return contact.Id;
        }

        static void Get_all_should_result_6_results()
        {
            //arrange
            var repository = CreateRepository();

            //act
            var contacts = repository.GetAll();

            //assert
            Console.WriteLine($"Count : {contacts.Count}");
            Console.WriteLine($"Is 6 result :{contacts.Count == 6}");
           
        }
        private static void Initialize()
        {
            //if you create appsettings.json => right click + change to copy if newer
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config = builder.Build();
        }

        private static IContactRepository CreateRepository()
        {
            //return new ContactRepository(config.GetConnectionString("DefaultConnection"));
            return new ContactRepositorySP(config.GetConnectionString("DefaultConnection"));
        }

    }
}
