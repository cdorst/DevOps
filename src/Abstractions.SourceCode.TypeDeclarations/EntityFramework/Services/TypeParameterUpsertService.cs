﻿using DevOps.Abstractions.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class TypeParameterUpsertService<TDbContext> : UpsertService<TDbContext, TypeParameter>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, Identifier> _identifiers;

        public TypeParameterUpsertService(ICacheService<TypeParameter> cache, TDbContext database, ILogger<UpsertService<TDbContext, TypeParameter>> logger, IUpsertService<TDbContext, Identifier> identifiers)
            : base(cache, database, logger, database.TypeParameters)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(TypeParameter)}={record.IdentifierId}";
            _identifiers = identifiers ?? throw new ArgumentNullException(nameof(identifiers));
        }

        protected override async Task<TypeParameter> AssignUpsertedReferences(TypeParameter record)
        {
            record.Identifier = await _identifiers.UpsertAsync(record.Identifier);
            record.IdentifierId = record.Identifier?.IdentifierId ?? record.IdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(TypeParameter record)
        {
            yield return record.Identifier;
        }

        protected override Expression<Func<TypeParameter, bool>> FindExisting(TypeParameter record)
            => existing => existing.IdentifierId == record.IdentifierId;
    }
}
