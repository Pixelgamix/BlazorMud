using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace BlazorMud.BusinessLogic.Services
{
    public sealed class CharacterService : ICharacterService
    {
        private readonly ILogger<AccountService> _Logger;
        private readonly IDatabaseContext _DatabaseContext;
        private readonly IMapper _Mapper;

        public CharacterService(ILogger<AccountService> logger, IDatabaseContext databaseContext, IMapper mapper)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResult<CharacterInfoModel[]>> ListCharacters(Guid accountId)
        {
            try
            {
                CharacterInfoModel[] characters = null;
                await _DatabaseContext.ExecuteAsync(async u =>
                    {
                        var dbCharacters = await u.CharacterRepository.ListCharactersByAccountIdAsync(accountId);
                        if (dbCharacters != null)
                            characters = dbCharacters.Select(x => _Mapper.Map<CharacterInfoModel>(x)).ToArray();
                    });
                return new ServiceResult<CharacterInfoModel[]>(true, result: characters ?? new CharacterInfoModel[0]);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error trying to fetch characters of account id {0}", accountId);
                return new ServiceResult<CharacterInfoModel[]>(false, "Server error.");
            }
        }
    }
}