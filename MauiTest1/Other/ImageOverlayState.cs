using System.Collections.ObjectModel;

namespace MauiTest1
{
    public class ImageOverlayState
    {
        private ObservableCollection<ImageOverlayBlock> images;

        public ImageOverlayState()
        {
            this.images = new ObservableCollection<ImageOverlayBlock>();
        }

        public ObservableCollection<ImageOverlayBlock> Images { get { return this.images; } }

        public int CreateImageBlock(int x, int y, int width, int height, string source) {
            ImageOverlayBlock newImageBlock = new(x, y, width, height, source);
            images.Add(newImageBlock);
            return newImageBlock.ID;
        }

        public void SetImageBlockSource(int id, string source) { 
            ImageOverlayBlock imageBlock = images.FirstOrDefault(x => x.ID == id);

            if (imageBlock != null)
            {
                ImageOverlayBlock newBlock = imageBlock.Clone();
                newBlock.SetImageSource(source);
                images.Remove(imageBlock);
                images.Add(newBlock);
            }
        }

        public void RemoveImageBlock(int id) {
            ImageOverlayBlock imageBlock = images.FirstOrDefault(x => x.ID == id);

            if (imageBlock != null)
            {
                images.Remove(imageBlock);
            }
        }

        public void Reset()
        {
            this.images.Clear();
        }
    }
}
