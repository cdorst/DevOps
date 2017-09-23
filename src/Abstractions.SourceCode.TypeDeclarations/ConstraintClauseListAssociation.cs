﻿using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations
{
    [ProtoContract]
    [Table("ConstraintClauseListAssociations", Schema = nameof(SourceCode))]
    public class ConstraintClauseListAssociation
    {
        [Key]
        [ProtoMember(1)]
        public int ConstraintClauseListAssociationId { get; set; }

        [ProtoMember(2)]
        public ConstraintClause ConstraintClause { get; set; }
        [ProtoMember(3)]
        public int ConstraintClauseId { get; set; }

        [ProtoMember(4)]
        public ConstraintClauseList ConstraintClauseList { get; set; }
        [ProtoMember(5)]
        public int ConstraintClauseListId { get; set; }
    }
}
