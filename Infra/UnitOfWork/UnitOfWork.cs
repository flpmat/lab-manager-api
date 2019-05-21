using System;
using System.Collections.Generic;
using Infra.Dapper;
using Infra.Repository._BaseRepository;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.UnitOfWork
{
        public class UnitOfWork<TContext> : 
            IRepositoryFactory, 
            IUnitOfWork<TContext> where TContext : DbContext, IDisposable
        {
            private Dictionary<Type, object> _repositories;
            public TContext Context { get; }
            private readonly DapperBaseConnection _dapperBaseConnection;

            public UnitOfWork(TContext context, DapperBaseConnection dapperBaseConnection)
            {
                Context = context ?? throw new ArgumentNullException(nameof(context));
                _dapperBaseConnection = dapperBaseConnection;
        }

            public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
            {
                if (_repositories == null) _repositories = new Dictionary<Type, object>();

                var type = typeof(TEntity);
                if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(Context);
                return (IRepository<TEntity>)_repositories[type];
            }

            public IRepositoryDapper<TEntity> GetRepositoryDapper<TEntity>() where TEntity : class
            {
                if (_repositories == null) _repositories = new Dictionary<Type, object>();
                var type = typeof(TEntity);
                if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryDapper<TEntity>(_dapperBaseConnection);
                return (IRepositoryDapper<TEntity>)_repositories[type];
            }

            public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
            {
                if (_repositories == null) _repositories = new Dictionary<Type, object>();

                var type = typeof(TEntity);
                if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryAsync<TEntity>(Context);
                return (IRepositoryAsync<TEntity>)_repositories[type];
            }

            public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class
            {
                if (_repositories == null) _repositories = new Dictionary<Type, object>();

                var type = typeof(TEntity);
                if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryReadOnly<TEntity>(Context);
                return (IRepositoryReadOnly<TEntity>)_repositories[type];
            }

            

            public int SaveChanges()
            {
                return Context.SaveChanges();
            }

            public void Dispose()
            {
                Context?.Dispose();
            }
        }
}
