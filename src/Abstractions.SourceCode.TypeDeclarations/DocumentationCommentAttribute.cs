﻿using DevOps.Abstractions.Core;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("DocumentationCommentAttributes", Schema = nameof(SourceCode))]
    public class DocumentationCommentAttribute : IUniqueListRecord
    {
        [Key]
        [ProtoMember(1)]
        public int DocumentationCommentAttributeId { get; set; }

        [ProtoMember(2)]
        public Identifier Identifier { get; set; }
        [ProtoMember(3)]
        public int IdentifierId { get; set; }

        [ProtoMember(4)]
        public Identifier Value { get; set; }
        [ProtoMember(5)]
        public int ValueId { get; set; }

        public static DocumentationCommentAttribute FromXmlAttributeSyntax(XmlAttributeSyntax syntax)
        {
            if (syntax is XmlCrefAttributeSyntax) return FromXmlCref(syntax);
            if (syntax is XmlNameAttributeSyntax) return FromXmlName(syntax);
            return FromXmlText(syntax as XmlTextAttributeSyntax);
        }

        private static DocumentationCommentAttribute FromXmlCref(XmlAttributeSyntax syntax)
            => new DocumentationCommentAttribute
            {
                Identifier = new Identifier { Name = new AsciiStringReference { Value = "cref" } },
                Value = new Identifier { Name = new AsciiStringReference { Value = (syntax as XmlCrefAttributeSyntax).Cref.ToString() } }
            };

        private static DocumentationCommentAttribute FromXmlName(XmlAttributeSyntax syntax)
            => new DocumentationCommentAttribute
            {
                Identifier = new Identifier { Name = new AsciiStringReference { Value = "name" } },
                Value = new Identifier { Name = new AsciiStringReference { Value = (syntax as XmlNameAttributeSyntax).Identifier.ToString() } }
            };

        private static DocumentationCommentAttribute FromXmlText(XmlTextAttributeSyntax syntax)
            => new DocumentationCommentAttribute
            {
                Identifier = new Identifier { Name = new AsciiStringReference { Value = syntax.Name.ToString() } },
                Value = new Identifier { Name = new AsciiStringReference { Value = syntax.TextTokens.ToString() } }
            };

        public XmlAttributeSyntax GetXmlAttributeSyntax()
            => string.Equals(Identifier.Name.Value, "cref", StringComparison.OrdinalIgnoreCase)
            ? GetXmlCrefAttributeSyntax()
            : string.Equals(Identifier.Name.Value, "name", StringComparison.OrdinalIgnoreCase)
            ? GetXmlNameAttributeSyntax()
            : GetXmlTextAttributeSyntax();

        private XmlAttributeSyntax GetXmlCrefAttributeSyntax()
            => XmlCrefAttribute(
                NameMemberCref(
                    ParseName(Value.Name.Value)));

        private XmlAttributeSyntax GetXmlNameAttributeSyntax()
            => XmlNameAttribute(
                XmlName(
                    Identifier.GetSyntaxToken()),
                Token(SyntaxKind.DoubleQuoteToken),
                Value.GetIdentifierNameSyntax(),
                Token(SyntaxKind.DoubleQuoteToken));

        private XmlAttributeSyntax GetXmlTextAttributeSyntax()
            => XmlTextAttribute(
                XmlName(
                    Identifier.GetSyntaxToken()),
                Token(SyntaxKind.DoubleQuoteToken),
                Token(SyntaxKind.DoubleQuoteToken))
            .WithTextTokens(
                TokenList(
                    XmlTextLiteral(
                        TriviaList(),
                        Value.Name.Value,
                        Value.Name.Value,
                        TriviaList())));
    }
}
