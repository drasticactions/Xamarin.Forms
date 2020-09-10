﻿using System;
using Xamarin.Platform.Core;

namespace Xamarin.Platform {
	public interface IViewRenderer
	{
		void SetView (IFrameworkElement view);
		void UpdateValue (string property);
		void Remove (IFrameworkElement view);
		object NativeView { get; }
		bool HasContainer { get; set; } 
		//ContainerView ContainerView { get; }
		SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint);
		void SetFrame (Rectangle frame);
	}
}