using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.interfaces.Repository
{
    public interface IApplicationDbConnectionFactory
    {
        IDbConnection CreateConnection();

    }
}
