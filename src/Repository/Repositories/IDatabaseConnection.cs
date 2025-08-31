using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IDatabaseConnection : IDisposable
    {
        public IDbConnection Connection { get; set; }

        public IDbTransaction Transaction { get; set; }

        Task EndTransactionAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync();
    }
}
