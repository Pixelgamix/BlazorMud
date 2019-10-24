using System.ComponentModel.DataAnnotations;

namespace BlazorMud.Contracts.DomainModel
{
    /// <summary>
    /// Domain model for account registration.
    /// </summary>
    public sealed class AccountRegistrationModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username is too short (min. 4 letters).")]
        [MaxLength(32, ErrorMessage = "Username is too long (max. 32 letters).")]
        [RegularExpression("[a-zA-Z]", ErrorMessage = "Username may contain letters only.")]
        public string AccountName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password is too short. Please use 8 characters at minimum.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mismatching password and repeated password.")]
        [MinLength(8, ErrorMessage = "Password is too short. Please use 8 characters at minimum.")]
        public string PasswordRepeat { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please read and accept the rules")]
        public bool HasReadAndAcceptedRules { get; set; }
    }
}
