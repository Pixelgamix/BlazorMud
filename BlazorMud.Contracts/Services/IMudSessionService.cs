using System;
using BlazorMud.Contracts.DomainModel;

namespace BlazorMud.Contracts.Services
{
    /// <summary>
    /// Manages MUD sessions.
    /// </summary>
    public interface IMudSessionService
    {
        /// <summary>
        /// Called as session is added.
        /// </summary>
        event EventHandler<MudSessionModel> SessionAdded;
        
        /// <summary>
        /// Called as a session is removed.
        /// </summary>
        event EventHandler<MudSessionModel> SessionRemoved; 
        
        /// <summary>
        /// Adds a session.
        /// </summary>
        /// <param name="session">The session to add.</param>
        ServiceResult AddSession(MudSessionModel session);
        
        /// <summary>
        /// Returns all sessions. 
        /// </summary>
        /// <returns>Array of all sessions.</returns>
        ServiceResult<MudSessionModel[]> ListSessions();
        
        /// <summary>
        /// Removes a session.
        /// </summary>
        /// <param name="session">The session to remove.</param>
        ServiceResult RemoveSession(MudSessionModel session);
    }
}