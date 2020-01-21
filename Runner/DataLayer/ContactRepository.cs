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
            var sql = "SELECT * FROM Contacts WHERE Id = @id";
            var contact = _db.Query<Contact>(sql, new{ id }).SingleOrDefault();
            return contact;
        }

        public List<Contact> GetAll()
        {
            return _db.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public Contact Add(Contact contact)
        {
            //the second line of sql is mean get the identity we have just inserted
            var sql =
                "INSERT INTO Contacts (FirstName,LastName,Email,Company,Title) VALUES  (@FirstName,@LastName,@Email,@Company,@Title)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = _db.Query<int>(sql, contact).Single();
            contact.Id = id;
            return contact;
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