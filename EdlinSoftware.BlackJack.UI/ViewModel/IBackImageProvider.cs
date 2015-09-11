using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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

        private int _imageWidth;
        private int _imageHeight;

        private int _backLeft;
        private int _backTop;

        private int _cardWidth;
        private int _cardHeight;

        private Size _cardSize;

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
            var lineIndex = 0;

            _backImage = new BitmapImage(new Uri(Path.Combine(backDirectory, descriptionLines[lineIndex++])));

            _imageWidth = _backImage.PixelWidth;
            _imageHeight = _backImage.PixelHeight;

            var backParts = descriptionLines[lineIndex++].Split(' ');
            _backLeft = int.Parse(backParts[0]);
            _backTop = int.Parse(backParts[1]);

            var cardSizeParts = descriptionLines[lineIndex].Split(' ');
            _cardWidth = int.Parse(cardSizeParts[0]);
            _cardHeight = int.Parse(cardSizeParts[1]);

            _cardSize = new Size((double)_cardWidth / _imageWidth, (double)_cardHeight / _imageHeight);
        }

        public BitmapImage GetImageForBack()
        {
            return _backImage;
        }

        public Rect GetViewboxForBack()
        {
            return new Rect(new Point((double)_backLeft / _imageWidth, (double)_backTop / _imageHeight), _cardSize);
        }
    }
}