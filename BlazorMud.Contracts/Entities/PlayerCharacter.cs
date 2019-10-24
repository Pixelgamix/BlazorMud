using System;

namespace BlazorMud.Contracts.Entities
{
    public class PlayerCharacter
    {
        public virtual Account Account { get; set; }
        public virtual Guid PlayerCharacterId { get; set; }
        public virtual string Forename { get; set; }
        public virtual string Surname { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime LastSelected { get; set; }  
    }
}