using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using EdlinSoftware.BlackJack.Annotations;
using EdlinSoftware.Cards;

namespace EdlinSoftware.BlackJack.UI.ViewModel
{
    public interface ICardImagesProvider
    {
        double GetCardImageRatio();

        BitmapImage GetImageForCard(Card card);
        Rect GetViewboxForCard(Card card);

        BitmapImage GetImageForBack();
        Rect GetViewboxForBack();
    }

    public class FileBasedCardImagesProvider : ICardImagesProvider
    {
        private readonly IBackImageProvider _backImageProvider;

        private double _ratio;
        private Size _cardSize;

        private BitmapImage _cardsImage;

        private double _firstCardHorizontalOffset;
        private double _firstCardVerticalOffset;

        private double _cardWidth;
        private double _cardHeight;

        private double _horizontalSpacing;
        private double _verticalSpacing;

        private Ranks[] _ranks;
        private Suits[] _suits;

        private int _imageWidth;
        private int _imageHeight;

        public FileBasedCardImagesProvider(string deckDescriptionFilePath,
            [NotNull] IBackImageProvider backImageProvider)
        {
            if (backImageProvider == null) throw new ArgumentNullException(nameof(backImageProvider));
            _backImageProvider = backImageProvider;

            deckDescriptionFilePath = Path.Combine(@".\Decks\", Path.ChangeExtension(deckDescriptionFilePath, ".deck"));

            if (!File.Exists(deckDescriptionFilePath))
                throw new ArgumentException($"There is no file '{deckDescriptionFilePath}'.", nameof(deckDescriptionFilePath));

            var fullPath = Path.GetFullPath(deckDescriptionFilePath);

            ParseDescriptionFile(Path.GetDirectoryName(fullPath), File.ReadAllLines(deckDescriptionFilePath));
        }

        private void ParseDescriptionFile(string deckDirectory, string[] descriptionLines)
        {
            var lineIndex = 0;

            _cardsImage = new BitmapImage(new Uri(Path.Combine(deckDirectory, descriptionLines[lineIndex++])));

            _imageWidth = _cardsImage.PixelWidth;
            _imageHeight = _cardsImage.PixelHeight;

            var offsetParts = descriptionLines[lineIndex++].Split(' ');
            _firstCardHorizontalOffset = double.Parse(offsetParts[0], CultureInfo.InvariantCulture);
            _firstCardVerticalOffset = double.Parse(offsetParts[1], CultureInfo.InvariantCulture);

            var cardSizeParts = descriptionLines[lineIndex++].Split(' ');
            _cardWidth = double.Parse(cardSizeParts[0], CultureInfo.InvariantCulture);
            _cardHeight = double.Parse(cardSizeParts[1], CultureInfo.InvariantCulture);

            _ratio = _cardWidth / _cardHeight;

            _cardSize = new Size(_cardWidth / _imageWidth, _cardHeight / _imageHeight);

            var spacingParts = descriptionLines[lineIndex++].Split(' ');
            _horizontalSpacing = double.Parse(spacingParts[0], CultureInfo.InvariantCulture);
            _verticalSpacing = double.Parse(spacingParts[1], CultureInfo.InvariantCulture);

            SetRanks(descriptionLines[lineIndex++]);

            SetSuits(descriptionLines[lineIndex]);
        }

        private void SetRanks(string ranksLine)
        {
            var ranks = ranksLine.Split(' ');

            _ranks = new Ranks[ranks.Length];

            var index = 0;
            foreach (var rank in ranks)
            {
                switch (rank.ToUpperInvariant())
                {
                    case "A":
                        _ranks[index++] = Ranks.Ace;
                        break;
                    case "2":
                        _ranks[index++] = Ranks.Two;
                        break;
                    case "3":
                        _ranks[index++] = Ranks.Three;
                        break;
                    case "4":
                        _ranks[index++] = Ranks.Four;
                        break;
                    case "5":
                        _ranks[index++] = Ranks.Five;
                        break;
                    case "6":
                        _ranks[index++] = Ranks.Six;
                        break;
                    case "7":
                        _ranks[index++] = Ranks.Seven;
                        break;
                    case "8":
                        _ranks[index++] = Ranks.Eight;
                        break;
                    case "9":
                        _ranks[index++] = Ranks.Nine;
                        break;
                    case "10":
                        _ranks[index++] = Ranks.Ten;
                        break;
                    case "J":
                        _ranks[index++] = Ranks.Jack;
                        break;
                    case "Q":
                        _ranks[index++] = Ranks.Queen;
                        break;
                    case "K":
                        _ranks[index++] = Ranks.King;
                        break;
                }
            }
        }

        private void SetSuits(string suitsLine)
        {
            var suits = suitsLine.Split(' ');

            _suits = new Suits[suits.Length];

            var index = 0;
            foreach (var suit in suits)
            {
                switch (suit.ToUpperInvariant())
                {
                    case "H":
                        _suits[index++] = Suits.Hearts;
                        break;
                    case "C":
                        _suits[index++] = Suits.Clubs;
                        break;
                    case "D":
                        _suits[index++] = Suits.Diamonds;
                        break;
                    case "S":
                        _suits[index++] = Suits.Spades;
                        break;
                }
            }
        }

        public double GetCardImageRatio()
        {
            return _ratio;
        }

        public BitmapImage GetImageForCard(Card card)
        {
            return _cardsImage;
        }

        public Rect GetViewboxForCard(Card card)
        {
            var hIndex = Array.IndexOf(_ranks, card.Rank);
            var vIndex = Array.IndexOf(_suits, card.Suit);

            var left = _firstCardHorizontalOffset + hIndex*(_cardWidth + _horizontalSpacing);
            var top = _firstCardVerticalOffset + vIndex * (_cardHeight + _verticalSpacing);

            return new Rect(new Point(left / _imageWidth, top / _imageHeight), _cardSize);
        }

        public BitmapImage GetImageForBack()
        {
            return _backImageProvider.GetImageForBack();
        }

        public Rect GetViewboxForBack()
        {
            return _backImageProvider.GetViewboxForBack();
        }
    }
}