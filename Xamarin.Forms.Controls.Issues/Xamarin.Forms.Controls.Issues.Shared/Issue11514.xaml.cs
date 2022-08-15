using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11514, "[Bug] iOS crashes when clipping with GeometryGroup", PlatformAffected.iOS)]
	public partial class Issue11514 : TestContentPage
	{
		public Issue11514()
		{
#if APP
			InitializeComponent();
#endif
		}

		protected override void Init()
		{

		}
	}
}