using Axiom.Application.Interfaces.Database;
using Axiom.Infrastructure.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Axiom.Infrastructure.Features.Client.Queries
{
    public class GetClientByIdQuery : IRequest<GetClientByIdResponse>
    {
        public string ClientId { get; set; }

        internal class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, GetClientByIdResponse>
        {
            private readonly IDapper _dapper;
            private string _connectionString;
            public GetClientByIdQueryHandler(IDataContext dataContext)
            {
                _dapper = dataContext.Dapper;
                _connectionString = dataContext.UserService.ConnectionString;
            }

            public async Task<GetClientByIdResponse> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
            {
                List<Tuple<string, string>> args = new List<Tuple<string, string>> { new Tuple<string, string>("ClientId", query.ClientId) };
                var eObject = await _dapper.ExecuteProcedureAsync(_connectionString, "ClientHeaderInfoAxiom", args);
                var result = eObject?.Select(s => new GetClientByIdResponse(s.Value)).FirstOrDefault();

                return result;
            }
        }
    }
}
