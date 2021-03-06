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
    [Table("ParameterLists", Schema = nameof(SourceCode))]
    public class ParameterList : IUniqueList<Parameter, ParameterListAssociation>
    {
        [Key]
        [ProtoMember(1)]
        public int ParameterListId { get; set; }

        [ProtoMember(2)]
        public AsciiStringReference ListIdentifier { get; set; }
        [ProtoMember(3)]
        public int ListIdentifierId { get; set; }

        [ProtoMember(4)]
        public List<ParameterListAssociation> ParameterListAssociations { get; set; }

        public ParameterListSyntax GetParameterListSyntax()
        {
            if (ParameterListAssociations.Count == 1)
            {
                return ParameterList(
                    SingletonSeparatedList(
                        ParameterListAssociations
                            .First()
                            .Parameter
                            .GetParameterSyntax()));
            }
            var last = ParameterListAssociations.Count - 1;
            var list = new List<SyntaxNodeOrToken>();
            for (int i = 0; i < last + 1; i++)
            {
                list.Add(
                    ParameterListAssociations[i]
                        .Parameter
                        .GetParameterSyntax());
                if (i != last) list.Add(Token(SyntaxKind.CommaToken));
            }
            return ParameterList(
                SeparatedList<ParameterSyntax>(
                    list));
        }

        public List<ParameterListAssociation> GetAssociations() => ParameterListAssociations;

        public void SetRecords(List<Parameter> records)
        {
            for (int i = 0; i < ParameterListAssociations.Count; i++)
            {
                ParameterListAssociations[i].SetRecord(records[i]);
            }
            ListIdentifier = new AsciiStringReference(
                string.Join(",", records.Select(r => r.ParameterId)));
        }
    }
}
