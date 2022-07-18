using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Axiom.Application.Interfaces.Database
{
    public interface ISqlConnectionProvider
    {
        IDbConnection GetDbConnection { get; }
        IDbTransaction GetTransaction { get; }
    }
}
