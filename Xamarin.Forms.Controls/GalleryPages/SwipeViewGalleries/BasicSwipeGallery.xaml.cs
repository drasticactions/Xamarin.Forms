using System;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.SwipeViewGalleries
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public partial class BasicSwipeGallery : ContentPage
	{
		public BasicSwipeGallery()
		{
			InitializeComponent();
		}

		private void OnInvoked(object sender, EventArgs e)
		{
			DisplayAlert("SwipeView", "Delete Invoked", "OK");
		}
	}
}