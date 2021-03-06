﻿using DevOps.Abstractions.Core;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("FieldLists", Schema = nameof(SourceCode))]
    public class FieldList : IUniqueList<Field, FieldListAssociation>
    {
        [Key]
        [ProtoMember(1)]
        public int FieldListId { get; set; }

        [ProtoMember(2)]
        public AsciiStringReference ListIdentifier { get; set; }
        [ProtoMember(3)]
        public int ListIdentifierId { get; set; }

        [ProtoMember(4)]
        public List<FieldListAssociation> FieldListAssociations { get; set; }

        public IEnumerable<MemberDeclarationSyntax> GetMemberDeclarationSyntax()
            => MemberDeclarationSorter.Sort(FieldListAssociations.Select(f => f.Field));

        public List<FieldListAssociation> GetAssociations() => FieldListAssociations;

        public void SetRecords(List<Field> records)
        {
            for (int i = 0; i < FieldListAssociations.Count; i++)
            {
                FieldListAssociations[i].SetRecord(records[i]);
            }
            ListIdentifier = new AsciiStringReference(
                string.Join(",", records.Select(r => r.FieldId)));
        }
    }
}
