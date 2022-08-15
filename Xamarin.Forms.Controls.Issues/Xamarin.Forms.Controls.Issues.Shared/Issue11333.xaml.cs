﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11333,
		"[Bug] SwipeView does not work on Android if child has TapGestureRecognizer",
		PlatformAffected.Android)]
	public partial class Issue11333 : TestContentPage
	{
		const string SwipeViewId = "SwipeViewId";

		public Issue11333()
		{
#if APP
			InitializeComponent();
#endif
		}

		protected override void Init()
		{
		}

#if APP
		void OnTapGestureRecognizerOnTapped(object sender, EventArgs e)
		{
			Debug.WriteLine("Tapped");
		}

		void OnSwipeViewSwipeEnded(object sender, SwipeEndedEventArgs e)
		{
			ResultLabel.Text = e.IsOpen ? "Open" : "Close";
		}
#endif

#if UITEST && __ANDROID__

		[Test]
		[Category(UITestCategories.SwipeView)]
		public void SwipeWithChildGestureRecognizer()
		{
			RunningApp.WaitForElement(SwipeViewId);
			RunningApp.SwipeRightToLeft();
			RunningApp.Tap(SwipeViewId);
			RunningApp.WaitForElement(q => q.Marked("Open"));
		}
#endif
	}

	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class Issue11333Model
	{
		public string Title { get; set; }
		public string Description { get; set; }
	}
}