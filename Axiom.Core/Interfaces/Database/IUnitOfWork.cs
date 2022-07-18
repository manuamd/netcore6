using Axiom.Application.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Application.Interfaces.Database
{
    public interface IUnitOfWork
    {
        IDapper _dapper { get; }
        void Commit();
        void Rollback();
    }
}
