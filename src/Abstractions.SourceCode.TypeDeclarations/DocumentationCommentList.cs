﻿using DevOps.Abstractions.Core;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("DocumentationCommentLists", Schema = nameof(SourceCode))]
    public class DocumentationCommentList : IUniqueList<DocumentationComment, DocumentationCommentListAssociation>
    {
        [Key]
        [ProtoMember(1)]
        public int DocumentationCommentListId { get; set; }

        [ProtoMember(2)]
        public bool IncludeNewLine { get; set; }

        [ProtoMember(3)]
        public AsciiStringReference ListIdentifier { get; set; }
        [ProtoMember(4)]
        public int ListIdentifierId { get; set; }

        [ProtoMember(5)]
        public List<DocumentationCommentListAssociation> DocumentationComments { get; set; }

        public DocumentationCommentTriviaSyntax GetDocumentationCommentTriviaSyntax()
            => DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                List(GetXmlNodeSyntaxList()));

        private IEnumerable<XmlNodeSyntax> GetXmlNodeSyntaxList()
        {
            var enumeration = DocumentationComments.SelectMany(c => c.DocumentationComment.GetXmlNodeSyntaxList());
            if (!IncludeNewLine) return enumeration;
            var list = enumeration.ToList();
            list.Add(GetNewLine());
            return list;
        }

        private static XmlTextSyntax GetNewLine()
            => XmlText()
                .WithTextTokens(
                    TokenList(
                        XmlTextNewLine(
                            TriviaList(),
                            Environment.NewLine,
                            Environment.NewLine,
                            TriviaList())));

        public List<DocumentationCommentListAssociation> GetAssociations() => DocumentationComments;

        public void SetRecords(List<DocumentationComment> records)
        {
            for (int i = 0; i < DocumentationComments.Count; i++)
            {
                DocumentationComments[i].SetRecord(records[i]);
            }
            ListIdentifier = new AsciiStringReference(
                string.Join(",", records.Select(r => r.DocumentationCommentId)));
        }
    }
}
