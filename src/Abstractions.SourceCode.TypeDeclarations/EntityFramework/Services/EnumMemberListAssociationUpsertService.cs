﻿using DevOps.Abstractions.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class EnumMemberListAssociationUpsertService<TDbContext> : UpsertService<TDbContext, EnumMemberListAssociation>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        public EnumMemberListAssociationUpsertService(ICacheService<EnumMemberListAssociation> cache, TDbContext database, ILogger<UpsertService<TDbContext, EnumMemberListAssociation>> logger)
            : base(cache, database, logger, database.EnumMemberListAssociations)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(EnumMemberListAssociation)}={record.EnumMemberId}:{record.EnumMemberListId}";
        }

        protected override IEnumerable<object> EnumerateReferences(EnumMemberListAssociation record)
        {
            yield return record.EnumMember;
            yield return record.EnumMemberList;
        }

        protected override Expression<Func<EnumMemberListAssociation, bool>> FindExisting(EnumMemberListAssociation record)
            => existing
                => existing.EnumMemberId == record.EnumMemberId
                && existing.EnumMemberListId == record.EnumMemberListId;
    }
}
