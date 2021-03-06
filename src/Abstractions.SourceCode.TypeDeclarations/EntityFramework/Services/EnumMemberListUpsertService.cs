﻿using DevOps.Abstractions.Core.Services;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class EnumMemberListUpsertService<TDbContext> : UpsertService<TDbContext, EnumMemberList>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, AsciiStringReference> _strings;

        public EnumMemberListUpsertService(ICacheService<EnumMemberList> cache, TDbContext database, ILogger<UpsertService<TDbContext, EnumMemberList>> logger, IUpsertService<TDbContext, AsciiStringReference> strings)
            : base(cache, database, logger, database.EnumMemberLists)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Expression)}={record.ListIdentifierId}";
            _strings = strings ?? throw new ArgumentNullException(nameof(strings));
        }

        protected override async Task<EnumMemberList> AssignUpsertedReferences(EnumMemberList record)
        {
            record.ListIdentifier = await _strings.UpsertAsync(record.ListIdentifier);
            record.ListIdentifierId = record.ListIdentifier?.AsciiStringReferenceId ?? record.ListIdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(EnumMemberList record)
        {
            yield return record.EnumMemberListAssociations;
            yield return record.ListIdentifier;
        }

        protected override Expression<Func<EnumMemberList, bool>> FindExisting(EnumMemberList record)
            => existing => existing.ListIdentifierId == record.ListIdentifierId;
    }
}
