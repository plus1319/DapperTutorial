using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace DataLayer
{
    public class ContactRepositorySP : IContactRepository
    {
        private readonly IDbConnection _db;
        public ContactRepositorySP(string connString)
        {
            _db = new SqlConnection(connString);
        }
        public Contact Find(int id)
        {
            return _db.Query<Contact>("SP_Find_Contact", new {id}, commandType: CommandType.StoredProcedure)
                .SingleOrDefault();
        }

        public List<Contact> GetAll()
        {
            return _db.Query<Contact>("SP_GetAll_Contacts", commandType: CommandType.StoredProcedure).ToList();
        }

        public Contact Add(Contact contact)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", value: contact.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parameters.Add("@FirstName",contact.FirstName);
            parameters.Add("@LastName",contact.LastName);
            parameters.Add("@Company", contact.Company);
            parameters.Add("@Email", contact.Email);
            parameters.Add("@Title", contact.Title);

            _db.Execute("SP_Add_Contact", parameters, commandType: CommandType.StoredProcedure);
            var contactId = parameters.Get<int>("@Id");

            contact.Id = contactId;
             return contact;
        }

        public Contact Update(Contact contact)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", contact.Id);
            parameters.Add("@FirstName", contact.FirstName);
            parameters.Add("@LastName", contact.LastName);
            parameters.Add("@Company", contact.Company);
            parameters.Add("@Email", contact.Email);
            parameters.Add("@Title", contact.Title);

            _db.Execute("SP_Update_Contact", parameters, commandType: CommandType.StoredProcedure);
            return contact;
        }

        public void Remove(int id)
        {
            _db.Execute("SP_Delete_Contact", new { id }, commandType: CommandType.StoredProcedure);
        }

        public Contact GetFullContact(int id)
        {
            using (var multipleResults = _db.QueryMultiple("SP_GetContact_Contact", new { id },
                commandType: CommandType.StoredProcedure))
            {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();
                var addresses = multipleResults.Read<Address>().ToList();

                contact?.Addresses?.AddRange(addresses);

                return contact;
            }
        }
    }
}
