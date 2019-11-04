using System;

namespace BlazorMud.Contracts.DomainModel
{
    /// <summary>
    /// Account information.
    /// </summary>
    public class AccountInfoModel
    {
        /// <summary>
        /// The account's unique id.
        /// </summary>
        public Guid AccountId { get; set; }
        
        /// <summary>
        /// The account's username.
        /// </summary>
        public string AccountName { get; set; }
    }
}