using Microsoft.Maui.Layouts;

namespace MauiTest1
{
    public class ImageOverlayBlock : AbsoluteLayout
    {
        // This property is shared by all objects of this type
        private static int lastAssignedId = 0;

        private int id;
        private int x;
        private int y;
        private int width;
        private int height;
        private string source;
        private Image image;

        public ImageOverlayBlock(int id, int x, int y, int width, int height, string source)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.source = source;

            Image image = new Image();
            this.image = image;
            this.image.Source = ImageSource.FromFile(source);
            this.image.Aspect = Aspect.Center;
            AbsoluteLayout.SetLayoutBounds(this.image, new Rect(0.5, 0.5, width, height));
            AbsoluteLayout.SetLayoutFlags(this.image, AbsoluteLayoutFlags.PositionProportional);
            this.Children.Add(image);
        }

        public ImageOverlayBlock(int x, int y, int width, int height, string source)
            : this(Interlocked.Increment(ref lastAssignedId), x, y, width, height, source) {}

        public int ID { get { return this.id; } }
        public int XPos { get { return this.x; } }
        public int YPos { get { return this.y; } }
        public int BlockWidth { get { return this.width; } }
        public int BlockHeight { get { return this.height; } }

        public void SetImageSource(string source)
        {
            this.image.Source = ImageSource.FromFile(source);
        }

        public ImageOverlayBlock Clone()
        {
            return new ImageOverlayBlock(this.id, this.x, this.y, this.width, this.height, this.source);
        }

        public static void ResetIDCounter()
        {
            lastAssignedId = 0;
        }
    }
}
