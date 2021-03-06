﻿using System;
using System.Collections.Generic;

namespace BlazorMud.Contracts.Entities
{
    public class Account
    {
        public virtual Guid AccountId { get; set; }
        public virtual string AccountName { get; set; }
        public virtual string HashedPassword { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual ISet<PlayerCharacter> Characters { get; set; }
    }
}
