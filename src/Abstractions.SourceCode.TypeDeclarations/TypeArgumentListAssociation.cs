﻿using DevOps.Abstractions.Core;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("TypeArgumentListAssociations", Schema = nameof(SourceCode))]
    public class TypeArgumentListAssociation : IUniqueListAssociation<TypeArgument>
    {
        [Key]
        [ProtoMember(1)]
        public int TypeArgumentListAssociationId { get; set; }

        [ProtoMember(2)]
        public TypeArgument TypeArgument { get; set; }
        [ProtoMember(3)]
        public int TypeArgumentId { get; set; }

        [ProtoMember(4)]
        public TypeArgumentList TypeArgumentList { get; set; }
        [ProtoMember(5)]
        public int TypeArgumentListId { get; set; }

        public TypeArgument GetRecord() => TypeArgument;

        public void SetRecord(TypeArgument record)
        {
            TypeArgument = record;
            TypeArgumentId = TypeArgument.TypeArgumentId;
        }
    }
}
