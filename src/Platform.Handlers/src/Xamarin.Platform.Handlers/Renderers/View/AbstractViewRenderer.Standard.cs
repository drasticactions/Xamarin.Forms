﻿using System;
using Xamarin.Platform.Core;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public abstract partial class AbstractViewRenderer<TVirtualView, TNativeView>
	{
		public void SetFrame(Rectangle rect)
		{

		}
		public virtual SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint)
			=> new SizeRequest ();

		void SetupContainer () { }
		void RemoveContainer () { }
	}
}
