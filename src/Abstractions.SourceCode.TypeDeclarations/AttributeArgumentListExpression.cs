﻿using DevOps.Abstractions.UniqueStrings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("AttributeArgumentListExpressions", Schema = nameof(SourceCode))]
    public class AttributeArgumentListExpression
    {
        [Key]
        [ProtoMember(1)]
        public int AttributeArgumentListExpressionId { get; set; }

        [ProtoMember(2)]
        public AsciiMaxStringReference Expression { get; set; }
        [ProtoMember(3)]
        public int ExpressionId { get; set; }

        public AttributeArgumentListSyntax GetAttributeArgumentListSyntax()
            => ParseAttributeArgumentList(Expression.Value);
    }
}
