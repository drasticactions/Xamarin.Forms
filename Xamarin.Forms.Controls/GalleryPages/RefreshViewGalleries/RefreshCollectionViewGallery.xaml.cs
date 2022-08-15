using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.RefreshViewGalleries
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public partial class RefreshCollectionViewGallery : ContentPage
	{
		public RefreshCollectionViewGallery()
		{
			InitializeComponent();
			BindingContext = new RefreshViewModel();
		}
	}
}