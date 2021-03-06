﻿using DevOps.Abstractions.UniqueStrings;
using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevOps.Abstractions.BusinessObjects
{
    [ProtoContract]
    [Table("Systems", Schema = nameof(BusinessObjects))]
    public class System
    {
        [Key]
        [ProtoMember(1)]
        public int SystemId { get; set; }
        
        [ProtoMember(2)]
        public AsciiStringReference Name { get; set; }
        [ProtoMember(3)]
        public int NameId { get; set; }
        
        [ProtoMember(4)]
        public List<Domain> Domains { get; set; }
    }
}
