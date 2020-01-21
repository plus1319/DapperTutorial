﻿using DataLayer;
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
            Get_all_should_result_6_results();



            Console.ReadLine();
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
