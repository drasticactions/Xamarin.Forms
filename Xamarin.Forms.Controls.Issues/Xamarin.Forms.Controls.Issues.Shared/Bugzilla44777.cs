﻿using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[NUnit.Framework.Category(Core.UITests.UITestCategories.Bugzilla)]
#endif
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 44777, "BarTextColor changes color for more than just the Navigation page")]
	public class Bugzilla44777 : TestFlyoutPage
	{
		protected override void Init()
		{
			Flyout = new ContentPage() { Title = "I am a master page" };
			Detail = new NavigationPage(new ContentPage { Content = new Label { Text = "The status bar text color on this page should be white on blue. When you show the Flyout page fully, the status bar text should be black on white. If the status bar text remains white when the Flyout page is fully presented, this test has failed." } });
			((NavigationPage)Detail).BarBackgroundColor = Color.Blue;
			((NavigationPage)Detail).BarTextColor = Color.White;

			IsPresentedChanged += (sender, e) =>
			{
				var mp = sender as FlyoutPage;
				if (mp.IsPresented)
					((NavigationPage)mp.Detail).On<iOS>().SetStatusBarTextColorMode(StatusBarTextColorMode.DoNotAdjust);
				else
					((NavigationPage)mp.Detail).On<iOS>().SetStatusBarTextColorMode(StatusBarTextColorMode.MatchNavigationBarTextLuminosity);
			};
		}
	}
}
