using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.RefreshViewGalleries
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public partial class RefreshLayoutGallery : ContentPage
	{
		public RefreshLayoutGallery()
		{
			InitializeComponent();
			BindingContext = new RefreshViewModel();
		}
	}
}