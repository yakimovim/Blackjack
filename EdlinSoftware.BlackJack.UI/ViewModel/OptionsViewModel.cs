using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EdlinSoftware.BlackJack.UI.Settings;

namespace EdlinSoftware.BlackJack.UI.ViewModel
{
    internal class OptionsViewModel : BaseViewModel
    {
        private readonly Configuration _configuration;
        private readonly BlackJackSection _configSection;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _deckFile;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _backFile;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _initialPlayerMoney;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _initialDealerMoney;

        public OptionsViewModel()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
            _configSection = (BlackJackSection)_configuration.GetSection("blackJack");
            _deckFile = _configSection.DeckFile;
            _backFile = _configSection.BackFile;
            _initialPlayerMoney = _configSection.InitialPlayerMoney;
            _initialDealerMoney = _configSection.InitialDealerMoney;
        }

        public int InitialDealerMoney
        {
            [DebuggerStepThrough]
            get
            { return _initialDealerMoney; }
            set
            {
                if (_initialDealerMoney != value)
                {
                    _initialDealerMoney = value;
                    OnPropertyChanged();
                }
            }
        }

        public int InitialPlayerMoney
        {
            [DebuggerStepThrough]
            get { return _initialPlayerMoney; }
            set
            {
                if (_initialPlayerMoney != value)
                {
                    _initialPlayerMoney = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool GameRestartRequired { get; set; }

        public string DeckFile
        {
            [DebuggerStepThrough]
            get { return _deckFile; }
            set
            {
                if (_deckFile != value)
                {
                    _deckFile = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<string> AllDeckFiles => Directory.GetFiles(@".\Decks", "*.deck")
            .Select(Path.GetFileNameWithoutExtension);

        public string BackFile
        {
            [DebuggerStepThrough]
            get { return _backFile; }
            set
            {
                if (_backFile != value)
                {
                    _backFile = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<string> AllBackFiles => Directory.GetFiles(@".\Backs", "*.back")
            .Select(Path.GetFileNameWithoutExtension);

        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(arg =>
                {
                    GameRestartRequired = _configSection.InitialPlayerMoney != InitialPlayerMoney 
                                       || _configSection.InitialDealerMoney != InitialDealerMoney;

                    _configSection.DeckFile = DeckFile;
                    _configSection.BackFile = BackFile;
                    _configSection.InitialPlayerMoney = InitialPlayerMoney;
                    _configSection.InitialDealerMoney = InitialDealerMoney;
                    _configuration.Save(ConfigurationSaveMode.Modified);
                });
            }
        }
    }
}
