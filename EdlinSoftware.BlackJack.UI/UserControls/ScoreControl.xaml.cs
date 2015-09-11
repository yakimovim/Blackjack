using System.Windows;
using System.Windows.Controls;

namespace EdlinSoftware.BlackJack.UI.UserControls
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl
    {
        public static DependencyProperty PlayerMoneyProperty;
        public static DependencyProperty DealerMoneyProperty;
        public static DependencyProperty CurrentBetProperty;

        static ScoreControl()
        {
            PropertyMetadata playerMoneyMetadata = new FrameworkPropertyMetadata(-1)
            {
                PropertyChangedCallback = OnPlayerScoreChanged
            };
            PlayerMoneyProperty = DependencyProperty.Register("PlayerMoney", typeof(int), typeof(ScoreControl), playerMoneyMetadata);

            PropertyMetadata dealerMoneyMetadata = new FrameworkPropertyMetadata(-1)
            {
                PropertyChangedCallback = OnDealerScoreChanged
            };
            DealerMoneyProperty = DependencyProperty.Register("DealerMoney", typeof(int), typeof(ScoreControl), dealerMoneyMetadata);

            PropertyMetadata currentBetMetadata = new FrameworkPropertyMetadata(-1)
            {
                PropertyChangedCallback = OnCurrentBetChanged
            };
            CurrentBetProperty = DependencyProperty.Register("CurrentBet", typeof(int), typeof(ScoreControl), currentBetMetadata);
        }

        public int PlayerMoney
        {
            get { return (int) GetValue(PlayerMoneyProperty); }
            set { SetValue(PlayerMoneyProperty, value); }
        }

        public int DealerMoney
        {
            get { return (int)GetValue(DealerMoneyProperty); }
            set { SetValue(DealerMoneyProperty, value); }
        }

        public int CurrentBet
        {
            get { return (int)GetValue(CurrentBetProperty); }
            set { SetValue(CurrentBetProperty, value); }
        }

        private static void OnPlayerScoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = string.Format(Properties.Resources.PlayersMoney, e.NewValue);

            var scoreControl = (ScoreControl) d;

            var size = TextSizeMeasurer.MeasureString(text, scoreControl.playerMoney);

            scoreControl.playerMoney.Text = text;

            Canvas.SetLeft(scoreControl.playerMoney, 1280 - size.Width - 10);
        }

        private static void OnDealerScoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = string.Format(Properties.Resources.DealersMoney, e.NewValue);

            ((ScoreControl)d).dealerMoney.Text = text;
        }

        private static void OnCurrentBetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = string.Format(Properties.Resources.CurrentBet, e.NewValue);

            var scoreControl = (ScoreControl)d;

            var size = TextSizeMeasurer.MeasureString(text, scoreControl.currentBet);

            scoreControl.currentBet.Text = text;

            Canvas.SetLeft(scoreControl.currentBet, (1280 - size.Width) / 2);
        }

        public ScoreControl()
        {
            InitializeComponent();
        }
    }
}
