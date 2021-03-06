﻿using DevOps.Abstractions.Core.Services;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class StatementListUpsertService<TDbContext> : UpsertService<TDbContext, StatementList>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, AsciiStringReference> _strings;

        public StatementListUpsertService(ICacheService<StatementList> cache, TDbContext database, ILogger<UpsertService<TDbContext, StatementList>> logger, IUpsertService<TDbContext, AsciiStringReference> strings)
            : base(cache, database, logger, database.StatementLists)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Expression)}={record.ListIdentifierId}";
            _strings = strings ?? throw new ArgumentNullException(nameof(strings));
        }

        protected override async Task<StatementList> AssignUpsertedReferences(StatementList record)
        {
            record.ListIdentifier = await _strings.UpsertAsync(record.ListIdentifier);
            record.ListIdentifierId = record.ListIdentifier?.AsciiStringReferenceId ?? record.ListIdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(StatementList record)
        {
            yield return record.StatementListAssociations;
            yield return record.ListIdentifier;
        }

        protected override Expression<Func<StatementList, bool>> FindExisting(StatementList record)
            => existing => existing.ListIdentifierId == record.ListIdentifierId;
    }
}
