using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorMud.Contracts.DomainModel
{
    /// <summary>
    /// A MUD user session.
    /// </summary>
    public class MudSessionModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The current user's account.
        /// </summary>
        public AccountInfoModel Account
        {
            get => _Account;
            set
            {
                if (value == _Account) return;
                _Account = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The selected character.
        /// </summary>
        public CharacterInfoModel Character
        {
            get => _Character;
            set
            {
                if (value == _Character) return;
                _Character = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private AccountInfoModel _Account;
        private CharacterInfoModel _Character;
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}