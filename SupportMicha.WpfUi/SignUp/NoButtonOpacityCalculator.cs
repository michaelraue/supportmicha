namespace SupportMicha.WpfUi.SignUp;

using System.Windows;

internal static class NoButtonOpacityCalculator
{
    private const double DistanceForNoButtonToBeInvisible = 10.0;
    private const double DistanceForNoButtonToBeCompletelyVisible = 75.0;

    internal static (double Opacity, Visibility Visibility) CalculateOpacityAndVisibility(
        Point point,
        Size buttonSize,
        Point buttonPosition)
    {
        var distance = CalculateDistance(point, buttonSize, buttonPosition);
        var opacity =
                (distance - DistanceForNoButtonToBeInvisible) /
                (DistanceForNoButtonToBeCompletelyVisible - DistanceForNoButtonToBeInvisible);
        var visibility = opacity < 0.01 ? Visibility.Hidden : Visibility.Visible;
        return (Math.Max(0, opacity), visibility);
    }

    private static double CalculateDistance(Point point, Size buttonSize, Point buttonPos)
    {
        var horizontalDistance = CalculateHorizontalDistance(point, buttonSize.Width, buttonPos.X);
        var verticalDistance = CalculateVerticalDistance(point, buttonSize.Height, buttonPos.Y);
        return Math.Sqrt(Math.Pow(horizontalDistance, 2) + Math.Pow(verticalDistance, 2));
    }

    private static double CalculateHorizontalDistance(Point point, double buttonWidth, double buttonX)
    {
        var left = buttonX;
        var right = buttonX + buttonWidth;
        if (point.X < left)
        {
            return left - point.X;
        }

        if (point.X > right)
        {
            return point.X - right;
        }

        return 0;
    }

    private static double CalculateVerticalDistance(Point point, double buttonHeight, double buttonY)
    {
        var top = buttonY;
        var bottom = buttonY + buttonHeight;
        if (point.Y < top)
        {
            return top - point.Y;
        }

        if (point.Y > bottom)
        {
            return point.Y - bottom;
        }

        return 0;
    }
}