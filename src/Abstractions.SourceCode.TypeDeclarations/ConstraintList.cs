﻿using DevOps.Abstractions.Core;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
    [Table("ConstraintLists", Schema = nameof(SourceCode))]
    public class ConstraintList : IUniqueList<Constraint, ConstraintListAssociation>
    {
        [Key]
        [ProtoMember(1)]
        public int ConstraintListId { get; set; }

        [ProtoMember(2)]
        public AsciiStringReference ListIdentifier { get; set; }
        [ProtoMember(3)]
        public int ListIdentifierId { get; set; }

        [ProtoMember(4)]
        public List<ConstraintListAssociation> ConstraintListAssociations { get; set; }

        public SeparatedSyntaxList<TypeParameterConstraintSyntax> GetConstraintList()
        {
            if (ConstraintListAssociations.Count == 1)
            {
                return SingletonSeparatedList(
                    ConstraintListAssociations
                        .First()
                        .Constraint
                        .GetTypeParameterConstraintSyntax());
            }
            var last = ConstraintListAssociations.Count - 1;
            var list = new List<SyntaxNodeOrToken>();
            for (int i = 0; i < last + 1; i++)
            {
                list.Add(
                    ConstraintListAssociations[i]
                        .Constraint
                        .GetTypeParameterConstraintSyntax());
                if (i != last) list.Add(Token(SyntaxKind.CommaToken));
            }
            return SeparatedList<TypeParameterConstraintSyntax>(list);
        }

        public List<ConstraintListAssociation> GetAssociations() => ConstraintListAssociations;

        public void SetRecords(List<Constraint> records)
        {
            for (int i = 0; i < ConstraintListAssociations.Count; i++)
            {
                ConstraintListAssociations[i].SetRecord(records[i]);
            }
            ListIdentifier = new AsciiStringReference(
                string.Join(",", records.Select(r => r.ConstraintId)));
        }
    }
}
