﻿using DevOps.Abstractions.Core;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("EnumMemberListAssociations", Schema = nameof(SourceCode))]
    public class EnumMemberListAssociation : IUniqueListAssociation<EnumMember>
    {
        [Key]
        [ProtoMember(1)]
        public int EnumMemberListAssociationId { get; set; }

        [ProtoMember(2)]
        public EnumMember EnumMember { get; set; }
        [ProtoMember(3)]
        public int EnumMemberId { get; set; }

        [ProtoMember(4)]
        public EnumMemberList EnumMemberList { get; set; }
        [ProtoMember(5)]
        public int EnumMemberListId { get; set; }

        public EnumMember GetRecord() => EnumMember;

        public void SetRecord(EnumMember record)
        {
            EnumMember = record;
            EnumMemberId = EnumMember.EnumMemberId;
        }
    }
}
