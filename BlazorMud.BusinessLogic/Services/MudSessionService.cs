using System;
using System.Linq;
using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Services;
using ConcurrentCollections;

namespace BlazorMud.BusinessLogic.Services
{
    public sealed class MudSessionService : IMudSessionService
    {
        private readonly ConcurrentHashSet<MudSessionModel> _Sessions = new ConcurrentHashSet<MudSessionModel>();

        public event EventHandler<MudSessionModel> SessionAdded;
        public event EventHandler<MudSessionModel> SessionRemoved; 
        
        public ServiceResult AddSession(MudSessionModel session)
        {
            if(_Sessions.Add(session))
                SessionAdded?.Invoke(this, session);
                
            return new ServiceResult();
        }

        public ServiceResult<MudSessionModel[]> ListSessions()
        {
            return new ServiceResult<MudSessionModel[]>(true, result: _Sessions.ToArray());
        }

        public ServiceResult RemoveSession(MudSessionModel session)
        {
            if(_Sessions.TryRemove(session))
                SessionRemoved?.Invoke(this, session);
            
            return new ServiceResult();
        }
    }
}