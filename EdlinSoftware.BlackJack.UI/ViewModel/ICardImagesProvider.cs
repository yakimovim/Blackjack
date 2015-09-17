using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using EdlinSoftware.BlackJack.Annotations;
using EdlinSoftware.Cards;
using EdlinSoftware.Cards.ImagePositions;
using Rect = System.Windows.Rect;
using CardSize = EdlinSoftware.Cards.ImagePositions.Size;

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

        private BitmapImage _cardsImage;
        private CardSize _imageSize;

        private RectangularCardImagePositionProvider _cardImagePositionProvider;

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
            var imageDescription = new RectangularDeckImageDescription();

            var lineIndex = 0;

            _cardsImage = new BitmapImage(new Uri(Path.Combine(deckDirectory, descriptionLines[lineIndex++])));

            _imageSize = new CardSize(_cardsImage.PixelWidth, _cardsImage.PixelHeight);

            var offsetParts = descriptionLines[lineIndex++].Split(' ');
            imageDescription.HorizontalOffsetOfFirstCard = double.Parse(offsetParts[0], CultureInfo.InvariantCulture);
            imageDescription.VerticalOffsetOfFirstCard = double.Parse(offsetParts[1], CultureInfo.InvariantCulture);

            var cardSizeParts = descriptionLines[lineIndex++].Split(' ');
            imageDescription.CardWidth = double.Parse(cardSizeParts[0], CultureInfo.InvariantCulture);
            imageDescription.CardHeight = double.Parse(cardSizeParts[1], CultureInfo.InvariantCulture);

            _ratio = imageDescription.CardWidth / imageDescription.CardHeight;

            var spacingParts = descriptionLines[lineIndex++].Split(' ');
            imageDescription.HorizontalSpacingBetweenCards = double.Parse(spacingParts[0], CultureInfo.InvariantCulture);
            imageDescription.VerticalSpacingBetweenCards = double.Parse(spacingParts[1], CultureInfo.InvariantCulture);

            imageDescription.Ranks = GetRanks(descriptionLines[lineIndex++]);
            imageDescription.Suits = GetSuits(descriptionLines[lineIndex]);

            _cardImagePositionProvider = new RectangularCardImagePositionProvider(imageDescription);
        }

        private Ranks[] GetRanks(string ranksLine)
        {
            var ranks = ranksLine.Split(' ');

            var ranksArray = new Ranks[ranks.Length];

            var index = 0;
            foreach (var rank in ranks)
            {
                switch (rank.ToUpperInvariant())
                {
                    case "A":
                        ranksArray[index++] = Ranks.Ace;
                        break;
                    case "2":
                        ranksArray[index++] = Ranks.Two;
                        break;
                    case "3":
                        ranksArray[index++] = Ranks.Three;
                        break;
                    case "4":
                        ranksArray[index++] = Ranks.Four;
                        break;
                    case "5":
                        ranksArray[index++] = Ranks.Five;
                        break;
                    case "6":
                        ranksArray[index++] = Ranks.Six;
                        break;
                    case "7":
                        ranksArray[index++] = Ranks.Seven;
                        break;
                    case "8":
                        ranksArray[index++] = Ranks.Eight;
                        break;
                    case "9":
                        ranksArray[index++] = Ranks.Nine;
                        break;
                    case "10":
                        ranksArray[index++] = Ranks.Ten;
                        break;
                    case "J":
                        ranksArray[index++] = Ranks.Jack;
                        break;
                    case "Q":
                        ranksArray[index++] = Ranks.Queen;
                        break;
                    case "K":
                        ranksArray[index++] = Ranks.King;
                        break;
                }
            }

            return ranksArray;
        }

        private Suits[] GetSuits(string suitsLine)
        {
            var suits = suitsLine.Split(' ');

            var suitsArray = new Suits[suits.Length];

            var index = 0;
            foreach (var suit in suits)
            {
                switch (suit.ToUpperInvariant())
                {
                    case "H":
                        suitsArray[index++] = Suits.Hearts;
                        break;
                    case "C":
                        suitsArray[index++] = Suits.Clubs;
                        break;
                    case "D":
                        suitsArray[index++] = Suits.Diamonds;
                        break;
                    case "S":
                        suitsArray[index++] = Suits.Spades;
                        break;
                }
            }

            return suitsArray;
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
            return _cardImagePositionProvider.GetCardRectangle(card).Normalize(_imageSize);
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