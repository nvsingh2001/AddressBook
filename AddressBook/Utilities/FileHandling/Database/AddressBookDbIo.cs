using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AddressBook.Utilities.FileHandling.Database;

public class AddressBookDbIo : IAddressBookIo
{
    private readonly string _connectionString;

    public AddressBookDbIo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SaveDataAsync(AddressBookService addressBookService)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await EnsureSchemaExistsAsync(connection);

        foreach (var book in addressBookService.AddressBooks)
        {
            // Upsert AddressBook
            // Note: This is a simplified logic. In production, use MERGE or stored procedures.
            // For this implementation, we'll check if it exists, insert if not, and get ID.
            
            var addressBookId = await GetOrCreateAddressBookId(connection, book.Key);

            foreach (var contact in book.Value)
            {
               await UpsertContact(connection, contact, addressBookId);
            }
        }
    }

    private async Task EnsureSchemaExistsAsync(SqlConnection connection)
    {
        var checkTableQuery = "SELECT CASE WHEN OBJECT_ID('dbo.AddressBook', 'U') IS NOT NULL THEN 1 ELSE 0 END";
        using var checkCmd = new SqlCommand(checkTableQuery, connection);
        var exists = (int)await checkCmd.ExecuteScalarAsync();

        if (exists == 0)
        {
            var createScript = @"
                CREATE TABLE AddressBook (
                    AddressBookId INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(255) NOT NULL UNIQUE
                );

                CREATE TABLE Contact (
                    ContactId INT IDENTITY(1,1) PRIMARY KEY,
                    AddressBookId INT FOREIGN KEY REFERENCES AddressBook(AddressBookId),
                    FirstName NVARCHAR(255),
                    LastName NVARCHAR(255),
                    Phone NVARCHAR(50),
                    Email NVARCHAR(255),
                    Address NVARCHAR(MAX),
                    City NVARCHAR(100),
                    State NVARCHAR(100),
                    Zip NVARCHAR(20)
                );
            ";

            using var createCmd = new SqlCommand(createScript, connection);
            await createCmd.ExecuteNonQueryAsync();
        }
    }

    private async Task<int> GetOrCreateAddressBookId(SqlConnection connection, string name)
    {
        var checkCmd = new SqlCommand("SELECT AddressBookId FROM AddressBook WHERE Name = @Name", connection);
        checkCmd.Parameters.AddWithValue("@Name", name);
        var result = await checkCmd.ExecuteScalarAsync();

        if (result != null) return (int)result;

        var insertCmd = new SqlCommand("INSERT INTO AddressBook (Name) OUTPUT INSERTED.AddressBookId VALUES (@Name)", connection);
        insertCmd.Parameters.AddWithValue("@Name", name);
        return (int)(await insertCmd.ExecuteScalarAsync() ?? 0);
    }

    private async Task UpsertContact(SqlConnection connection, Contact contact, int addressBookId)
    {
        var checkQuery = @"
            SELECT ContactId FROM Contact 
            WHERE FirstName = @FirstName AND LastName = @LastName AND AddressBookId = @AddressBookId";

        using var checkCmd = new SqlCommand(checkQuery, connection);
        checkCmd.Parameters.AddWithValue("@FirstName", contact.FirstName);
        checkCmd.Parameters.AddWithValue("@LastName", contact.LastName);
        checkCmd.Parameters.AddWithValue("@AddressBookId", addressBookId);

        var contactId = await checkCmd.ExecuteScalarAsync();

        if (contactId != null)
        {
             var updateQuery = @"
                UPDATE Contact SET 
                    Phone = @Phone, Email = @Email, Address = @Address, 
                    City = @City, State = @State, Zip = @Zip 
                WHERE ContactId = @ContactId";
            
            using var updateCmd = new SqlCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@Phone", contact.Phone);
            updateCmd.Parameters.AddWithValue("@Email", contact.Email);
            updateCmd.Parameters.AddWithValue("@Address", contact.Address);
            updateCmd.Parameters.AddWithValue("@City", contact.City);
            updateCmd.Parameters.AddWithValue("@State", contact.State);
            updateCmd.Parameters.AddWithValue("@Zip", contact.Zip);
            updateCmd.Parameters.AddWithValue("@ContactId", contactId);
            await updateCmd.ExecuteNonQueryAsync();
        }
        else
        {
             var insertQuery = @"
                INSERT INTO Contact (FirstName, LastName, Phone, Email, Address, City, State, Zip, AddressBookId)
                VALUES (@FirstName, @LastName, @Phone, @Email, @Address, @City, @State, @Zip, @AddressBookId)";
            
            using var insertCmd = new SqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@FirstName", contact.FirstName);
            insertCmd.Parameters.AddWithValue("@LastName", contact.LastName);
            insertCmd.Parameters.AddWithValue("@Phone", contact.Phone);
            insertCmd.Parameters.AddWithValue("@Email", contact.Email);
            insertCmd.Parameters.AddWithValue("@Address", contact.Address);
            insertCmd.Parameters.AddWithValue("@City", contact.City);
            insertCmd.Parameters.AddWithValue("@State", contact.State);
            insertCmd.Parameters.AddWithValue("@Zip", contact.Zip);
            insertCmd.Parameters.AddWithValue("@AddressBookId", addressBookId);
            await insertCmd.ExecuteNonQueryAsync();
        }
    }

    public async Task LoadDataAsync(AddressBookService addressBookService)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await EnsureSchemaExistsAsync(connection);

        var query = @"
            SELECT ab.Name as BookName, c.* 
            FROM Contact c
            JOIN AddressBook ab ON c.AddressBookId = ab.AddressBookId";

        using var cmd = new SqlCommand(query, connection);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var bookName = reader["BookName"].ToString() ?? "Default";
            
            ContactManager manager;
            if (!addressBookService.ContainsAddressBook(bookName))
            {
                manager = new ContactManager();
                addressBookService.AddAddressBook(bookName, manager);
            }
            else
            {
                manager = addressBookService.GetAddressBook(bookName);
            }

            var contact = new Contact(
                reader["FirstName"].ToString()!,
                reader["LastName"].ToString()!,
                reader["Phone"].ToString()!,
                reader["Email"].ToString()!,
                reader["Address"].ToString()!,
                reader["City"].ToString()!,
                reader["State"].ToString()!,
                reader["Zip"].ToString()!
            );

            if (!manager.ContainsContact(contact.FirstName + contact.LastName))
            {
                manager.AddContact(contact);
                addressBookService.AddContactByCityAndState(contact);
            }
        }
    }
}