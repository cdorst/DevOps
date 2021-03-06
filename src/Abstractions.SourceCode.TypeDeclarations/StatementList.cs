﻿using DevOps.Abstractions.Core;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("StatementLists", Schema = nameof(SourceCode))]
    public class StatementList : IUniqueList<Statement, StatementListAssociation>
    {
        [Key]
        [ProtoMember(1)]
        public int StatementListId { get; set; }

        [ProtoMember(2)]
        public AsciiStringReference ListIdentifier { get; set; }
        [ProtoMember(3)]
        public int ListIdentifierId { get; set; }

        [ProtoMember(4)]
        public List<StatementListAssociation> StatementListAssociations { get; set; }

        public SyntaxList<StatementSyntax> GetStatementListSyntax()
            => StatementListAssociations.Count == 1
            ? SingletonList(
                StatementListAssociations
                    .First()
                    .Statement
                    .GetStatementSyntax())
            : List(
                StatementListAssociations
                    .Select(s => s.Statement.GetStatementSyntax()));

        public List<StatementListAssociation> GetAssociations() => StatementListAssociations;

        public void SetRecords(List<Statement> records)
        {
            for (int i = 0; i < StatementListAssociations.Count; i++)
            {
                StatementListAssociations[i].SetRecord(records[i]);
            }
            ListIdentifier = new AsciiStringReference(
                string.Join(",", records.Select(r => r.StatementId)));
        }
    }
}
