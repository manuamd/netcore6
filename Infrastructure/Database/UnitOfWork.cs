using Axiom.Application.Interfaces.Database;
using Axiom.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Axiom.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IDapper _dapper { get; }
        private IDbTransaction _dbTransaction;
        public UnitOfWork(IDapper dapper, IDbTransaction dbTransaction) 
        {
            _dapper = dapper;
            _dbTransaction = dbTransaction;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }

        public void Dispose()
        {
            //Close the SQL Connection and dispose the objects
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
