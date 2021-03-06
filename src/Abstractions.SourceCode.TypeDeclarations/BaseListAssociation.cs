﻿using DevOps.Abstractions.Core;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("BaseListAssociations", Schema = nameof(SourceCode))]
    public class BaseListAssociation : IUniqueListAssociation<BaseType>
    {
        [Key]
        [ProtoMember(1)]
        public int BaseListAssociationId { get; set; }

        [ProtoMember(2)]
        public BaseType BaseType { get; set; }
        [ProtoMember(3)]
        public int BaseTypeId { get; set; }

        [ProtoMember(4)]
        public BaseList BaseList { get; set; }
        [ProtoMember(5)]
        public int BaseListId { get; set; }

        public BaseType GetRecord() => BaseType;

        public void SetRecord(BaseType record)
        {
            BaseType = record;
            BaseTypeId = BaseType.BaseTypeId;
        }
    }
}
