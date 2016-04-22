using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ica.website.Models;
using ica.website.Infrastructure.Tasks;
using System.Data.Entity;

namespace ica.website.Infrastructure
{
    public class TransactionPerRequest : IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
        private IBaseOperations _dbContext;
        private HttpContextBase _httpContext;

        public TransactionPerRequest(IBaseOperations dbContext, HttpContextBase httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        void IRunOnEachRequest.Execute()
        {
            _httpContext.Items["_Transaction"] =
                _dbContext.GetTranscaiton().Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        void IRunOnError.Execute()
        {
            _httpContext.Items["_Error"] = true;
        }

        void IRunAfterEachRequest.Execute()
        {
            var transaction = (DbContextTransaction)_httpContext.Items["_Transaction"];

            if (_httpContext.Items["_Error"] != null)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
        }
    }
}