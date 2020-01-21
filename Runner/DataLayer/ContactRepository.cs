using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DataLayer
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDbConnection _db;
        public ContactRepository(string connString)
        {
            _db = new SqlConnection(connString);
        }
        public Contact Find(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Contact> GetAll()
        {
            return _db.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public Contact Add(Contact contact)
        {
            throw new System.NotImplementedException();
        }

        public Contact Update(Contact contact)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}