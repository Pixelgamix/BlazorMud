namespace BlazorMud.Contracts.Security
{
    public sealed class SecuritySettings
    {
        public Tokens Tokens { get; set; }
    }

    public sealed class Tokens
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
