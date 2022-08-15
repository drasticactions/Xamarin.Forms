using System;
using System.Linq;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.PerformanceGallery.Scenarios
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	internal class EditorScenario1 : PerformanceScenario
	{
		public EditorScenario1()
		: base("[Editor] Empty")
		{
			View = new Editor();
		}
	}
}
