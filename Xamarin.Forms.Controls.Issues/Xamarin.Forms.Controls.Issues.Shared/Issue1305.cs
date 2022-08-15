﻿using System;

using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[NUnit.Framework.Category(Core.UITests.UITestCategories.Github5000)]
#endif
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 1305, "ToolbarItems on NavigationPage broken", PlatformAffected.Android, NavigationBehavior.PushAsync)]
	public class Issue1305 : ContentPage
	{
		public Issue1305()
		{
			var settings = new ToolbarItem
			{
				IconImageSource = "bank.png",
				Text = "Settings",
				Command = new Command(ShowSettingsPage),
			};

			ToolbarItems.Add(settings);

			Content = new ContentView { Content = new Label { Text = "Testing..." } };

			Title = "Test Page";

			IconImageSource = "Icon.png";

		}

		async void ShowSettingsPage()
		{
			await Navigation.PushAsync(new Issue13052());
		}
	}

	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 1305, "ToolbarItems on NavigationPage broken", PlatformAffected.Android)]
	public class Issue13052 : ContentPage
	{
		public Issue13052()
		{
			Content = new ContentView { Content = new Label { Text = "Settings..." } };

			Title = "Settings";

			IconImageSource = "bank.png";
		}
	}
}

