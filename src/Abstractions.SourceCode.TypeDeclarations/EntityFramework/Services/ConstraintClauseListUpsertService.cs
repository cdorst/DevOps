﻿using DevOps.Abstractions.Core.Services;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class ConstraintClauseListUpsertService<TDbContext> : UpsertService<TDbContext, ConstraintClauseList>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, AsciiStringReference> _strings;

        public ConstraintClauseListUpsertService(ICacheService<ConstraintClauseList> cache, TDbContext database, ILogger<UpsertService<TDbContext, ConstraintClauseList>> logger, IUpsertService<TDbContext, AsciiStringReference> strings)
            : base(cache, database, logger, database.ConstraintClauseLists)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Expression)}={record.ListIdentifierId}";
            _strings = strings ?? throw new ArgumentNullException(nameof(strings));
        }

        protected override async Task<ConstraintClauseList> AssignUpsertedReferences(ConstraintClauseList record)
        {
            record.ListIdentifier = await _strings.UpsertAsync(record.ListIdentifier);
            record.ListIdentifierId = record.ListIdentifier?.AsciiStringReferenceId ?? record.ListIdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(ConstraintClauseList record)
        {
            yield return record.ConstraintClauseListAssociations;
            yield return record.ListIdentifier;
        }

        protected override Expression<Func<ConstraintClauseList, bool>> FindExisting(ConstraintClauseList record)
            => existing => existing.ListIdentifierId == record.ListIdentifierId;
    }
}
