using System.ComponentModel.DataAnnotations;

namespace BlazorMud.Contracts.DomainModel
{
    public sealed class AccountLoginModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "Username is too long.")]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password is too short. Please use 8 characters at minimum.")]
        public string Password { get; set; }

        [Required]
        [Range(60, 1440, ErrorMessage = "Login must expire minimum within 1 hour and maximum within 24 hours.")]
        public int ExpireMinutes { get; set; } = 60;

        // HACK: Workaround for Blazor InputSelect not yet supporting integers...
        public string ExpireMinutesAsString
        {
            get => ExpireMinutes.ToString();
            set => ExpireMinutes = int.Parse(value);
        }
    }
}
