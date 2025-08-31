using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _databaseConnection;

        public DatabaseConnection(string databaseConnection)
        {
            _databaseConnection = databaseConnection;
            Connection = CreateConnection();
        }

        ~DatabaseConnection()
        {
            Dispose(false);
        }

        public IDbConnection Connection { get; set; }

        public IDbTransaction Transaction { get; set; }

        public Task BeginTransactionAsync()
        {
            Transaction = Connection.BeginTransaction();

            return Task.CompletedTask;
        }

        public Task EndTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction.Dispose();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Connection?.Dispose();
            }
        }

        private IDbConnection CreateConnection()
        {
            var mySqlConnection = new MySqlConnection(_databaseConnection);
            mySqlConnection.Open();
            if (mySqlConnection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Database could not be opened");
            }

            return mySqlConnection;
        }
    }
}
