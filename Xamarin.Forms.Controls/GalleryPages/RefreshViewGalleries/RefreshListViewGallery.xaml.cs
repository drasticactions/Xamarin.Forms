using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.RefreshViewGalleries
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public partial class RefreshListViewGallery : ContentPage
	{
		public RefreshListViewGallery()
		{
			InitializeComponent();
			BindingContext = new RefreshViewModel();
		}
	}
}
