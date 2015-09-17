using System;
using System.IO;
using System.Windows.Media.Imaging;
using EdlinSoftware.Cards.ImagePositions;
using Rect = System.Windows.Rect;
using CardSize = EdlinSoftware.Cards.ImagePositions.Size;

namespace EdlinSoftware.BlackJack.UI.ViewModel
{
    public interface IBackImageProvider
    {
        BitmapImage GetImageForBack();
        Rect GetViewboxForBack();
    }

    public class FileBasedBackImageProvider : IBackImageProvider
    {
        private BitmapImage _backImage;
        private Rect _viewBox;

        public FileBasedBackImageProvider(string backDescriptionFilePath)
        {
            backDescriptionFilePath = Path.Combine(@".\Backs\", Path.ChangeExtension(backDescriptionFilePath, ".back"));

            if (!File.Exists(backDescriptionFilePath))
                throw new ArgumentException($"There is no file '{backDescriptionFilePath}'.", nameof(backDescriptionFilePath));

            var fullPath = Path.GetFullPath(backDescriptionFilePath);

            ParseDescriptionFile(Path.GetDirectoryName(fullPath), File.ReadAllLines(backDescriptionFilePath));
        }

        private void ParseDescriptionFile(string backDirectory, string[] descriptionLines)
        {
            var oneCardImageDescription = new OneCardImageDescription();

            var lineIndex = 0;

            _backImage = new BitmapImage(new Uri(Path.Combine(backDirectory, descriptionLines[lineIndex++])));

            var imageWidth = _backImage.PixelWidth;
            var imageHeight = _backImage.PixelHeight;

            var backParts = descriptionLines[lineIndex++].Split(' ');
            oneCardImageDescription.HorizontalOffset = int.Parse(backParts[0]);
            oneCardImageDescription.VerticalOffset = int.Parse(backParts[1]);

            var cardSizeParts = descriptionLines[lineIndex].Split(' ');
            oneCardImageDescription.CardWidth = int.Parse(cardSizeParts[0]);
            oneCardImageDescription.CardHeight = int.Parse(cardSizeParts[1]);

            var oneCardImagePositionProvider = new OneCardImagePositionProvider(oneCardImageDescription);

            _viewBox = oneCardImagePositionProvider.GetCardRectangle().Normalize(new CardSize(imageWidth, imageHeight));
        }

        public BitmapImage GetImageForBack()
        {
            return _backImage;
        }

        public Rect GetViewboxForBack()
        {
            return _viewBox;
        }
    }
}