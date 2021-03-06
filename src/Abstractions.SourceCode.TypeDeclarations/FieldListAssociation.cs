﻿using DevOps.Abstractions.Core;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("FieldListAssociations", Schema = nameof(SourceCode))]
    public class FieldListAssociation : IUniqueListAssociation<Field>
    {
        [Key]
        [ProtoMember(1)]
        public int FieldListAssociationId { get; set; }

        [ProtoMember(2)]
        public Field Field { get; set; }
        [ProtoMember(3)]
        public int FieldId { get; set; }

        [ProtoMember(4)]
        public FieldList FieldList { get; set; }
        [ProtoMember(5)]
        public int FieldListId { get; set; }

        public Field GetRecord() => Field;

        public void SetRecord(Field record)
        {
            Field = record;
            FieldId = Field.FieldId;
        }
    }
}
