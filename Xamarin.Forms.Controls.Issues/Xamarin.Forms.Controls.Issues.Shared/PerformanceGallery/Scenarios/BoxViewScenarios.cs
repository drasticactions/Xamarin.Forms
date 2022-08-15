using System;
using System.Linq;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.PerformanceGallery.Scenarios
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	internal class BoxViewScenario1 : PerformanceScenario
	{
		public BoxViewScenario1()
		: base("[BoxView] Color set in ctor")
		{
			View = new BoxView { Color = Color.Red };
		}
	}
}
