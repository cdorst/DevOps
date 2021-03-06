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
    [Table("UsingDirectiveLists", Schema = nameof(SourceCode))]
    public class UsingDirectiveList : IUniqueList<UsingDirective, UsingDirectiveListAssociation>
    {
        [Key]
        [ProtoMember(1)]
        public int UsingDirectiveListId { get; set; }

        [ProtoMember(2)]
        public AsciiStringReference ListIdentifier { get; set; }
        [ProtoMember(3)]
        public int ListIdentifierId { get; set; }

        [ProtoMember(4)]
        public List<UsingDirectiveListAssociation> UsingDirectiveListAssociations { get; set; }

        public SyntaxList<UsingDirectiveSyntax> GetUsingDirectiveListSyntax()
            => UsingDirectiveListAssociations.Count == 1
            ? SingletonList(
                UsingDirectiveListAssociations
                    .First()
                    .UsingDirective
                    .GetUsingDirectiveSyntax())
            : List(
                UsingDirectiveListAssociations
                    .Select(s => s.UsingDirective.GetUsingDirectiveSyntax()));

        public List<UsingDirectiveListAssociation> GetAssociations() => UsingDirectiveListAssociations;

        public void SetRecords(List<UsingDirective> records)
        {
            for (int i = 0; i < UsingDirectiveListAssociations.Count; i++)
            {
                UsingDirectiveListAssociations[i].SetRecord(records[i]);
            }
            ListIdentifier = new AsciiStringReference(
                string.Join(",", records.Select(r => r.UsingDirectiveId)));
        }
    }
}
