using AppKit;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.MacOS
{
	internal static class FlowDirectionExtensions
	{
#if NET6_0_OR_GREATER
		internal static FlowDirection ToFlowDirection(this NSUserInterfaceLayoutDirection direction)
		{
			switch (direction)
			{
				case NSUserInterfaceLayoutDirection.LeftToRight:
					return FlowDirection.LeftToRight;
				case NSUserInterfaceLayoutDirection.RightToLeft:
					return FlowDirection.RightToLeft;
				default:
					return FlowDirection.MatchParent;
			}
		}
#else
		internal static FlowDirection ToFlowDirection(this NSApplicationLayoutDirection direction)
		{
			switch (direction)
			{
				case NSApplicationLayoutDirection.LeftToRight:
					return FlowDirection.LeftToRight;
				case NSApplicationLayoutDirection.RightToLeft:
					return FlowDirection.RightToLeft;
				default:
					return FlowDirection.MatchParent;
			}
		}
#endif

		internal static void UpdateFlowDirection(this NSView view, IVisualElementController controller)
		{
			if (controller == null || view == null)
				return;

			if (controller.EffectiveFlowDirection.IsRightToLeft())
				view.UserInterfaceLayoutDirection = NSUserInterfaceLayoutDirection.RightToLeft;
			else if (controller.EffectiveFlowDirection.IsLeftToRight())
				view.UserInterfaceLayoutDirection = NSUserInterfaceLayoutDirection.LeftToRight;
		}

		internal static void UpdateFlowDirection(this NSTextField control, IVisualElementController controller)
		{
			if (controller == null || control == null)
				return;

			if (controller.EffectiveFlowDirection.IsRightToLeft())
			{
				control.Alignment = NSTextAlignment.Right;
			}
			else if (controller.EffectiveFlowDirection.IsLeftToRight())
			{
				control.Alignment = NSTextAlignment.Left;
			}
		}
	}
}