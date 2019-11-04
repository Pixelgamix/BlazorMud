using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace BlazorMud.BusinessLogic.Services
{
    public sealed class CharacterService : ICharacterService
    {
        private readonly ILogger<AccountService> _Logger;
        private readonly IDatabaseContext _DatabaseContext;
        private readonly IMapper _Mapper;
        private readonly MudSessionModel _SessionModel;

        public CharacterService(ILogger<AccountService> logger, IDatabaseContext databaseContext, IMapper mapper,
            MudSessionModel sessionModel)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _SessionModel = sessionModel ?? throw new ArgumentNullException(nameof(sessionModel));
        }

        /// <summary>
        /// Creates a new character.
        /// </summary>
        /// <param name="characterCreationModel">The character to create.</param>
        /// <returns>Info on success/failure of character creation.</returns>
        public async Task<ServiceResult> CreateCharacterAsync(CharacterCreationModel characterCreationModel)
        {
            try
            {
                var playerCharacter = _Mapper.Map<PlayerCharacter>(characterCreationModel);
                await _DatabaseContext.ExecuteAsync(async u =>
                {
                    var account = await u.AccountRepository.GetAccountByIdAsync(_SessionModel.Account.AccountId);
                    playerCharacter.Account = account;
                    await u.CharacterRepository.AddNewCharacterAsync(playerCharacter);
                });
                return new ServiceResult(true);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Failed to create character {0}", characterCreationModel);
                return new ServiceResult<CharacterInfoModel[]>(false, "Server error.");
            }
        }

        public async Task<ServiceResult<CharacterInfoModel>> FetchCharacterById(Guid characterId)
        {
            try
            {
                PlayerCharacter playerCharacter = null;
                await _DatabaseContext.ExecuteAsync(async u =>
                    playerCharacter = await u.CharacterRepository.FetchCharacterById(characterId));

                var characterInfo = playerCharacter != null ? _Mapper.Map<CharacterInfoModel>(playerCharacter) : null;

                return new ServiceResult<CharacterInfoModel>(true, result: characterInfo);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Failed to fetch character with id {0}", characterId);
                return new ServiceResult<CharacterInfoModel>(false, "Server error.");
            }
        }

        public async Task<ServiceResult<CharacterInfoModel[]>> ListCharactersAsync(Guid accountId)
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