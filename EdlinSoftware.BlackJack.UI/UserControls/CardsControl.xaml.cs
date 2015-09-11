using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using EdlinSoftware.BlackJack.UI.ViewModel;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack.UI.UserControls
{
    /// <summary>
    /// Interaction logic for CardsControl.xaml
    /// </summary>
    public partial class CardsControl
    {
        public static readonly DependencyProperty CardsProperty;
        public static readonly DependencyProperty TitleProperty;
        public static readonly DependencyProperty FirtsCardShouldBeFlippedProperty;
        public static readonly DependencyProperty CardImagesProviderProperty;

        static CardsControl()
        {
            FrameworkPropertyMetadata cardsMetadata = new FrameworkPropertyMetadata(new Card[0])
            {
                PropertyChangedCallback = OnCardsChanged
            };

            CardsProperty = DependencyProperty.Register("Cards",
                typeof(IEnumerable<Card>),
                typeof(CardsControl),
                cardsMetadata);

            FrameworkPropertyMetadata titleMetadata = new FrameworkPropertyMetadata(string.Empty)
            {
                PropertyChangedCallback = OnTitleChanged
            };

            TitleProperty = DependencyProperty.Register("Title",
                typeof(string),
                typeof(CardsControl),
                titleMetadata);

            FrameworkPropertyMetadata firstCardShouldBeFlippedMetadata = new FrameworkPropertyMetadata(false)
            {
                PropertyChangedCallback = OnFirstCardShouldBeFlippedChanged
            };

            FirtsCardShouldBeFlippedProperty = DependencyProperty.Register("FirtsCardShouldBeFlipped",
                typeof(bool),
                typeof(CardsControl),
                firstCardShouldBeFlippedMetadata);

            FrameworkPropertyMetadata cardImagesProviderMetadata = new FrameworkPropertyMetadata(null)
            {
                PropertyChangedCallback = OnCardImagesProviderChanged
            };

            CardImagesProviderProperty = DependencyProperty.Register("CardImagesProvider",
                typeof(ICardImagesProvider),
                typeof(CardsControl),
                cardImagesProviderMetadata);
        }

        public ICardImagesProvider CardImagesProvider
        {
            get { return (ICardImagesProvider)GetValue(CardImagesProviderProperty); }
            set { SetValue(CardImagesProviderProperty, value); }
        }

        public IEnumerable<Card> Cards
        {
            get { return (IEnumerable<Card>)GetValue(CardsProperty); }
            set { SetValue(CardsProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool FirtsCardShouldBeFlipped
        {
            get { return (bool)GetValue(FirtsCardShouldBeFlippedProperty); }
            set { SetValue(FirtsCardShouldBeFlippedProperty, value); }
        }

        private static void OnCardImagesProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cardsControl = (CardsControl)d;

            cardsControl.UpdateCards(cardsControl.Cards);
        }

        private static void OnCardsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cardsControl = (CardsControl)d;

            var oldCards = e.OldValue as INotifyCollectionChanged;
            var newCards = e.NewValue as INotifyCollectionChanged;

            if (oldCards != null)
            {
                oldCards.CollectionChanged -= cardsControl.OnCardsCollectionChanged;
            }
            if (newCards != null)
            {
                newCards.CollectionChanged += cardsControl.OnCardsCollectionChanged;
            }

            cardsControl.UpdateCards((IEnumerable<Card>)e.NewValue);
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cardsControl = (CardsControl) d;

            cardsControl.caption.Text = (string) e.NewValue;

            cardsControl.UpdateCards(cardsControl.Cards);
        }

        private static void OnFirstCardShouldBeFlippedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cardsControl = (CardsControl)d;

            cardsControl.UpdateCards(cardsControl.Cards);
        }

        private void OnCardsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCards((IEnumerable<Card>)sender);
        }

        private void UpdateCards(IEnumerable<Card> cards)
        {
            var cardsRectangles = cardsCanvas.Children.OfType<Rectangle>().ToArray();

            foreach (var cardsRectangle in cardsRectangles)
            {
                cardsCanvas.Children.Remove(cardsRectangle);
            }

            var cardImagesProvider = CardImagesProvider;

            if (cards != null && cardImagesProvider != null)
            {
                var cardsArray = cards.ToArray();

                var height = 200.0;
                var width = height* cardImagesProvider.GetCardImageRatio();

                double left = 40 + TextSizeMeasurer.MeasureString(Title, caption).Width;

                var cardsFieldWidth = 1280 - left - 40;
                var distanceBetweenCards = Math.Min(20, 
                    cardsFieldWidth / cardsArray.Length - width);

                bool firstCard = true;
                foreach (var card in cardsArray)
                {
                    var cardShouldBeFlipped = firstCard && FirtsCardShouldBeFlipped;

                    var cardImage = cardShouldBeFlipped
                        ? cardImagesProvider.GetImageForBack()
                        : cardImagesProvider.GetImageForCard(card);
                    var cardViewbox = cardShouldBeFlipped
                        ? cardImagesProvider.GetViewboxForBack()
                        : cardImagesProvider.GetViewboxForCard(card);

                    var rectangle = new Rectangle
                    {
                        Width = width,
                        Height = height,
                        Fill = new ImageBrush
                        {
                            ImageSource = cardImage,
                            Viewbox = cardViewbox,
                            Stretch = cardShouldBeFlipped ? Stretch.Fill : Stretch.Uniform
                        }
                    };
                    Canvas.SetLeft(rectangle, left);
                    Canvas.SetTop(rectangle, 30);
                    cardsCanvas.Children.Add(rectangle);
                    left += width + distanceBetweenCards;
                    firstCard = false;
                }
            }
        }

        public CardsControl()
        {
            InitializeComponent();
        }
    }
}
