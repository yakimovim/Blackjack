using System.Windows;

namespace EdlinSoftware.BlackJack.UI.ViewModel
{
    internal static class PositionExtensions
    {
        public static Size Normalize(this Cards.ImagePositions.Size size, Cards.ImagePositions.Size totalSize)
        {
            return new Size(size.Width/totalSize.Width, size.Height/totalSize.Height);
        }

        public static Rect Normalize(this Cards.ImagePositions.Rect rect, Cards.ImagePositions.Size totalSize)
        {
            return new Rect(
                new Point(rect.Left / totalSize.Width, rect.Top / totalSize.Height), 
                rect.Size.Normalize(totalSize));
        }
    }
}