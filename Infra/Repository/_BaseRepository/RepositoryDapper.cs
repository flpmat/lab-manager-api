using System.Collections.Generic;
using Dapper;
using Infra.Dapper;
using Infra.Repository._BaseRepository.Interfaces;

namespace Infra.Repository._BaseRepository
{
    public class RepositoryDapper<T> : IRepositoryDapper<T> where T : class
    {
        private readonly DapperBaseConnection _dapperBaseConnection;

        public RepositoryDapper(DapperBaseConnection dapperBaseConnection)
        {
            _dapperBaseConnection = dapperBaseConnection;
        }

        /// <summary>
        ///  Query using Dapper
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public IEnumerable<T> GetData(string qry)
        {
            using (var conexao = _dapperBaseConnection.Connection)
            {

                conexao.Open();
                var multi = conexao.QueryMultiple(qry);
                var result = multi.Read<T>();
                return result;
            }

        }

    }
}
