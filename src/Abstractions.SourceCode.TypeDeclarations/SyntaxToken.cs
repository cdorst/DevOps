﻿using Microsoft.CodeAnalysis.CSharp;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("SyntaxTokens", Schema = nameof(SourceCode))]
    public class SyntaxToken
    {
        [Key]
        [ProtoMember(1)]
        public int SyntaxTokenId { get; set; }

        [ProtoMember(2)]
        public SyntaxKind SyntaxKind { get; set; }

        public Microsoft.CodeAnalysis.SyntaxToken GetToken()
            => Token(SyntaxKind);
    }
}