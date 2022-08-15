﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[NUnit.Framework.Category(Core.UITests.UITestCategories.Bugzilla)]
#endif
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 31255, "Flyout's page Icon cause memory leak after FlyoutPage is popped out by holding on page")]
	public class Bugzilla31255 : TestContentPage // or TestFlyoutPage, etc ...
	{
		protected override void Init()
		{
			var stack = new StackLayout() { VerticalOptions = LayoutOptions.Center };

			stack.Children.Add(new Label()
			{
				VerticalOptions =
#pragma warning disable 618
				LayoutOptions.Center,
				XAlign = TextAlignment.Center,
				Text = "Page 1"
#pragma warning restore 618
			});

			Content = stack;

		}

		WeakReference _page2Tracker;

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (_page2Tracker == null)
			{
				var page2 = new Page2();

				_page2Tracker = new WeakReference(page2, false);

				await Task.Delay(1000);
				await Navigation.PushModalAsync(page2);

				StartTrackPage2();
			}
		}

		async void StartTrackPage2()
		{
			while (true)
			{
				((Label)((StackLayout)Content).Children[0]).Text =
						string.Format("Page1. But Page2 IsAlive = {0}", _page2Tracker.IsAlive);
				await Task.Delay(1000);
				GarbageCollectionHelper.Collect();
			}
		}

		[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
		public class Page2 : FlyoutPage
		{
			public Page2()
			{
				Flyout = new Page()
				{
					Title = "Flyout",
					IconImageSource = "Icon.png"
				};
				Detail = new Page() { Title = "Detail" };
			}

			protected override async void OnAppearing()
			{
				base.OnAppearing();

				await Task.Delay(1000);
				await Navigation.PopModalAsync();
			}
		}

#if UITEST
		[Test]
		[Ignore("Fails intermittently on TestCloud")]
		public async Task Bugzilla31255Test()
		{
			RunningApp.Screenshot("I am at Bugzilla 31255");
			await Task.Delay(5000);
			RunningApp.WaitForElement(q => q.Marked("Page1. But Page2 IsAlive = False"));
		}
#endif
	}
}
