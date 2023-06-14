using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MauiTest1
{
    public class ImageOverlay : AbsoluteLayout
    {
        private ObservableCollection<ImageOverlayBlock> imageState;

        public ImageOverlay()
        {
            PropertyChanged += StateChanged;
        }

        public static readonly BindableProperty ImageOverlayStateProperty =
            BindableProperty.Create(
                nameof(ImageOverlayState), 
                typeof(ImageOverlayState), 
                typeof(ImageOverlay)
            );

        public ImageOverlayState ImageOverlayState
        {
            get { return (ImageOverlayState)GetValue(ImageOverlayStateProperty); }
            set { SetValue(ImageOverlayStateProperty, value); }
        }

        private void StateChanged(object sender, EventArgs e)
        {
            // Remove the event handler on the old collection before referencing
            // the new collection and adding an event handler to it.
            if (e is not PropertyChangedEventArgs args) return;
            if (args.PropertyName != "ImageOverlayState") return;

            if (this.imageState != null)
            {
                this.imageState.CollectionChanged -= ImageCollectionChanged;
            }
            
            this.imageState = ImageOverlayState.Images;
            this.imageState.CollectionChanged += ImageCollectionChanged;
        }

        public void ImageCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { 
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(ImageOverlayBlock newItem in e.NewItems)
                {
                    Rect position = new(newItem.XPos, newItem.YPos, newItem.BlockWidth, newItem.BlockHeight);
                    AbsoluteLayout.SetLayoutBounds(newItem, position);
                    newItem.SetValue(AutomationProperties.NameProperty, newItem.ID.ToString());
                    this.Children.Add(newItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ImageOverlayBlock oldItem in e.OldItems)
                {
                    var toRemove = this.Children.FirstOrDefault(child =>
                    {
                        string v = AutomationProperties.GetName((ImageOverlayBlock)child);
                        return v == oldItem.ID.ToString();
                    });

                    if (toRemove != null)
                    {
                        this.Children.Remove(toRemove);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.Children.Clear();
            }
        }
    }
}
