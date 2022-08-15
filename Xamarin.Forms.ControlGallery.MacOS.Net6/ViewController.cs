using AppKit;
using Foundation;
using ObjCRuntime;

namespace Xamarin.Forms.ControlGallery.MacOS.Net6;

public partial class ViewController : NSViewController {
	protected ViewController (NativeHandle handle) : base (handle)
	{
	}

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();

		// Do any additional setup after loading the view.
	}

	public override NSObject RepresentedObject {
		get => base.RepresentedObject;
		set {
			base.RepresentedObject = value;

			// Update the view, if already loaded.
		}
	}
}
