using System;
using FluentValidation;
using Domain.Entities;
using Domain.DTO;

namespace Services
{
    public class ClusterValidator : AbstractValidator<Domain.Entities.Cluster>
    {
        public ClusterValidator()
        {
            #region Generated Constructor
            // RuleFor(p => p.AnoExercicio).NotEmpty();
            // RuleFor(p => p.IdAreaAtuacao).NotEmpty();
            // RuleFor(p => p.IdTipoCluster).NotEmpty();
            // RuleFor(p => p.IdTipoSituacaoCluster).NotEmpty();
            #endregion
        }

    }
}
