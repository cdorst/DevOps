﻿using DevOps.Abstractions.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("Arguments", Schema = nameof(SourceCode))]
    public class Argument : IUniqueListRecord
    {
        [Key]
        [ProtoMember(1)]
        public int ArgumentId { get; set; }

        [ProtoMember(2)]
        public Identifier Identifier { get; set; }
        [ProtoMember(3)]
        public int IdentifierId { get; set; }

        public ArgumentSyntax GetArgumentSyntax()
            => Argument(
                Identifier.GetIdentifierNameSyntax());
    }
}
