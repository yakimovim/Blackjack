using System.Configuration;

namespace EdlinSoftware.BlackJack.UI.Settings
{
    public class BlackJackSection : ConfigurationSection
    {
        [ConfigurationProperty("deckFile", DefaultValue = @"Deck 1", IsRequired = true)]
        public string DeckFile
        {
            get { return (string)base["deckFile"]; }
            set { base["deckFile"] = value; }
        }

        [ConfigurationProperty("backFile", DefaultValue = @"Back 1", IsRequired = true)]
        public string BackFile
        {
            get { return (string)base["backFile"]; }
            set { base["backFile"] = value; }
        }

        [ConfigurationProperty("initialPlayerMoney", DefaultValue = 100, IsRequired = true)]
        public int InitialPlayerMoney
        {
            get { return (int)base["initialPlayerMoney"]; }
            set { base["initialPlayerMoney"] = value; }
        }

        [ConfigurationProperty("initialDealerMoney", DefaultValue = 100, IsRequired = true)]
        public int InitialDealerMoney
        {
            get { return (int)base["initialDealerMoney"]; }
            set { base["initialDealerMoney"] = value; }
        }
    }
}