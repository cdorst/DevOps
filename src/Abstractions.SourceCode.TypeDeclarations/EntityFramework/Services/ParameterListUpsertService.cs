﻿using DevOps.Abstractions.Core.Services;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class ParameterListUpsertService<TDbContext> : UpsertService<TDbContext, ParameterList>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, AsciiStringReference> _strings;

        public ParameterListUpsertService(ICacheService<ParameterList> cache, TDbContext database, ILogger<UpsertService<TDbContext, ParameterList>> logger, IUpsertService<TDbContext, AsciiStringReference> strings)
            : base(cache, database, logger, database.ParameterLists)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Expression)}={record.ListIdentifierId}";
            _strings = strings ?? throw new ArgumentNullException(nameof(strings));
        }

        protected override async Task<ParameterList> AssignUpsertedReferences(ParameterList record)
        {
            record.ListIdentifier = await _strings.UpsertAsync(record.ListIdentifier);
            record.ListIdentifierId = record.ListIdentifier?.AsciiStringReferenceId ?? record.ListIdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(ParameterList record)
        {
            yield return record.ParameterListAssociations;
            yield return record.ListIdentifier;
        }

        protected override Expression<Func<ParameterList, bool>> FindExisting(ParameterList record)
            => existing => existing.ListIdentifierId == record.ListIdentifierId;
    }
}
