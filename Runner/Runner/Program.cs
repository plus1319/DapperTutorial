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
            Find_should_retrieve_existing_entity(7);


            Console.ReadLine();
        }

        static void Find_should_retrieve_existing_entity(int id)
        {
            //arrange
            IContactRepository repository = CreateRepository();

            //act 
            var contact = repository.Find(id);

            //assert
            Console.WriteLine("*** Get Contact***");
            contact.Output();
            Debug.Assert(contact.FirstName == "Joe");
            Debug.Assert(contact.LastName == "Blow");
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
            Debug.Assert(contact.Id != 0);
            Console.WriteLine("*** Contact inserted ***");
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
            Debug.Assert(contacts.Count == 6);
            contacts.Output();
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
            return new ContactRepository(config.GetConnectionString("DefaultConnection"));
        }

    }
}
