using System;
using System.Linq;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.PerformanceGallery.Scenarios
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	internal class DatePickerScenario1 : PerformanceScenario
	{
		public DatePickerScenario1()
		: base("[DatePicker] Empty")
		{
			View = new DatePicker();
		}
	}
}
