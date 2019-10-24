using System;

namespace BlazorMud.Contracts.DomainModel
{
    /// <summary>
    /// Information about a character.
    /// </summary>
    public class CharacterInfoModel
    {
        /// <summary>
        /// The character's unique identifier.
        /// </summary>
        public Guid PlayerCharacterId { get; set; }
        
        /// <summary>
        /// The character's forename.
        /// </summary>
        public string Forename { get; set; }
        
        /// <summary>
        /// The character's surname.
        /// </summary>
        public string Surname { get; set; }
    }
}