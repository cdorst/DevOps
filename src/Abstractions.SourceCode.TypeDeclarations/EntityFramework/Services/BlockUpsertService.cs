﻿using DevOps.Abstractions.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class BlockUpsertService<TDbContext> : UpsertService<TDbContext, Block>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertUniqueListService<TDbContext, Statement, StatementList, StatementListAssociation> _statementLists;

        public BlockUpsertService(ICacheService<Block> cache, TDbContext database, ILogger<UpsertService<TDbContext, Block>> logger, IUpsertUniqueListService<TDbContext, Statement, StatementList, StatementListAssociation> statementLists)
            : base(cache, database, logger, database.Blocks)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Block)}={record.StatementListId}";
            _statementLists = statementLists ?? throw new ArgumentNullException(nameof(statementLists));
        }

        protected override async Task<Block> AssignUpsertedReferences(Block record)
        {
            record.StatementList = await _statementLists.UpsertAsync(record.StatementList);
            record.StatementListId = record.StatementList?.StatementListId ?? record.StatementListId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(Block record)
        {
            yield return record.StatementList;
        }

        protected override Expression<Func<Block, bool>> FindExisting(Block record)
            => existing => existing.StatementListId == record.StatementListId;
    }
}
