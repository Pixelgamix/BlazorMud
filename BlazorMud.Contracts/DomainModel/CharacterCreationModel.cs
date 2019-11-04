namespace BlazorMud.Contracts.DomainModel
{
    /// <summary>
    /// Information provided by the user for creating a character.
    /// </summary>
    public sealed class CharacterCreationModel
    {
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