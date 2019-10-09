using System.Collections.Generic;

namespace BlazorMud.Contracts.Database
{
    public sealed class DatabaseSettings
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}
