﻿using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;
using ColorStateList = Android.Content.Res.ColorStateList;
using IMenu = Android.Views.IMenu;
using LP = Android.Views.ViewGroup.LayoutParams;
using Orientation = Android.Widget.Orientation;
using R = Android.Resource;
using Typeface = Android.Graphics.Typeface;
using TypefaceStyle = Android.Graphics.TypefaceStyle;

namespace Xamarin.Forms.Platform.Android
{
	public class ShellBottomTabItemRenderer : ShellItemRendererBase, BottomNavigationView.IOnNavigationItemSelectedListener, IAppearanceObserver
	{
		#region IOnNavigationItemSelectedListener

		bool BottomNavigationView.IOnNavigationItemSelectedListener.OnNavigationItemSelected(IMenuItem item)
		{
			return OnItemSelected(item);
		}

		#endregion IOnNavigationItemSelectedListener

		#region IAppearanceObserver

		void IAppearanceObserver.OnAppearanceChanged(ShellAppearance appearance)
		{
			if (appearance != null)
				SetAppearance(appearance);
			else
				ResetAppearance();
		}

		#endregion IAppearanceObserver

		protected const int MoreTabId = 99;
		private BottomNavigationView _bottomView;
		private FrameLayout _navigationArea;
		private IShellBottomNavViewAppearanceTracker _appearanceTracker;

		public ShellBottomTabItemRenderer(IShellContext shellContext) : base(shellContext)
		{
		}

		public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var outerLayout = inflater.Inflate(Resource.Layout.BottomTabLayout, null);
			_bottomView = outerLayout.FindViewById<BottomNavigationView>(Resource.Id.bottomtab_tabbar);
			_navigationArea = outerLayout.FindViewById<FrameLayout>(Resource.Id.bottomtab_navarea);

			_bottomView.SetBackgroundColor(Color.White.ToAndroid());
			_bottomView.SetOnNavigationItemSelectedListener(this);

			HookEvents(ShellItem);
			SetupMenu();

			_appearanceTracker = ShellContext.CreateBottomNavViewAppearanceTracker(ShellItem);
			((IShellController)ShellContext.Shell).AddAppearanceObserver(this, ShellItem);

			return outerLayout;
		}

		// Use OnDestory become OnDestroyView may fire before events are completed.
		public override void OnDestroy()
		{
			UnhookEvents(ShellItem);
			if (_bottomView != null)
			{
				_bottomView.SetOnNavigationItemSelectedListener(null);
				_bottomView.Dispose();
				_bottomView = null;

				_appearanceTracker?.Dispose();
				_appearanceTracker = null;
			}

			((IShellController)ShellContext.Shell).RemoveAppearanceObserver(this);

			base.OnDestroy();
		}

		protected virtual void SetAppearance(ShellAppearance appearance) => _appearanceTracker.SetAppearance(_bottomView, appearance);

		protected virtual void ChangeTabItem(ShellTabItem tabItem)
		{
			var controller = (IShellController)ShellContext.Shell;
			bool accept = controller.ProposeNavigation(ShellNavigationSource.ShellTabItemChanged,
				ShellItem,
				tabItem,
				tabItem.Stack.ToList(),
				true
			);

			if (accept)
				ShellItem.SetValueFromRenderer(ShellItem.CurrentItemProperty, tabItem);
		}

		protected virtual Drawable CreateItemBackgroundDrawable()
		{
			var stateList = ColorStateList.ValueOf(Color.Black.MultiplyAlpha(0.2).ToAndroid());
			return new RippleDrawable(stateList, new ColorDrawable(AColor.White), null);
		}

		protected virtual BottomSheetDialog CreateMoreBottomSheet(Action<ShellTabItem, BottomSheetDialog> selectCallback)
		{
			var bottomSheetDialog = new BottomSheetDialog(Context);
			var bottomSheetLayout = new LinearLayout(Context);
			bottomSheetLayout.LayoutParameters = new LP(LP.MatchParent, LP.WrapContent);
			bottomSheetLayout.Orientation = Orientation.Vertical;
			// handle the more tab
			for (int i = 4; i < ShellItem.Items.Count; i++)
			{
				var tab = ShellItem.Items[i];

				var innerLayout = new LinearLayout(Context);
				innerLayout.ClipToOutline = true;
				innerLayout.SetBackground(CreateItemBackgroundDrawable());
				innerLayout.SetPadding(0, (int)Context.ToPixels(6), 0, (int)Context.ToPixels(6));
				innerLayout.Orientation = Orientation.Horizontal;
				innerLayout.LayoutParameters = new LP(LP.MatchParent, LP.WrapContent);

				// technically the unhook isn't needed
				// we dont even unhook the events that dont fire
				void clickCallback(object s, EventArgs e)
				{
					selectCallback(tab, bottomSheetDialog);
					innerLayout.Click -= clickCallback;
				}
				innerLayout.Click += clickCallback;

				var image = new ImageView(Context);
				image.LayoutParameters = new LinearLayout.LayoutParams((int)Context.ToPixels(32), (int)Context.ToPixels(32))
				{
					LeftMargin = (int)Context.ToPixels(20),
					RightMargin = (int)Context.ToPixels(20),
					TopMargin = (int)Context.ToPixels(6),
					BottomMargin = (int)Context.ToPixels(6),
					Gravity = GravityFlags.Center
				};
				image.ImageTintList = ColorStateList.ValueOf(Color.Black.MultiplyAlpha(0.6).ToAndroid());
				SetImage(image, tab.Icon);

				innerLayout.AddView(image);

				var text = new TextView(Context);
				text.SetTypeface(Typeface.Create("sans-serif-medium", TypefaceStyle.Normal), TypefaceStyle.Normal);
				text.SetTextColor(AColor.Black);
				text.Text = tab.Title;
				text.LayoutParameters = new LinearLayout.LayoutParams(0, LP.WrapContent)
				{
					Gravity = GravityFlags.Center,
					Weight = 1
				};

				innerLayout.AddView(text);

				bottomSheetLayout.AddView(innerLayout);
			}

			bottomSheetDialog.SetContentView(bottomSheetLayout);

			return bottomSheetDialog;
		}

		protected override ViewGroup GetNavigationTarget() => _navigationArea;

		protected override void OnCurrentTabItemChanged()
		{
			base.OnCurrentTabItemChanged();

			var index = ShellItem.Items.IndexOf(CurrentTabItem);
			index = Math.Min(index, _bottomView.Menu.Size() - 1);
			if (index < 0)
				return;
			var menuItem = _bottomView.Menu.GetItem(index);
			menuItem.SetChecked(true);
		}

		protected override void OnDisplayedPageChanged(Page newPage, Page oldPage)
		{
			base.OnDisplayedPageChanged(newPage, oldPage);

			if (oldPage != null)
				oldPage.PropertyChanged -= OnDisplayedPagePropertyChanged;

			if (newPage != null)
				newPage.PropertyChanged += OnDisplayedPagePropertyChanged;

			UpdateTabBarVisibility();
		}

		protected virtual bool OnItemSelected(IMenuItem item)
		{
			var id = item.ItemId;
			if (id == MoreTabId)
			{
				var bottomSheetDialog = CreateMoreBottomSheet(OnMoreItemSelected);
				bottomSheetDialog.Show();
				bottomSheetDialog.DismissEvent += OnMoreSheetDismissed;
			}
			else
			{
				var shellTabItem = ShellItem.Items[id];
				if (item.IsChecked)
				{
					OnTabReselected(shellTabItem);
				}
				else
				{
					ChangeTabItem(shellTabItem);
				}
			}

			return true;
		}

		protected virtual void OnMoreItemSelected(ShellTabItem tabItem, BottomSheetDialog dialog)
		{
			ChangeTabItem(tabItem);

			dialog.Dismiss();
			dialog.Dispose();
		}

		protected virtual void OnMoreSheetDismissed(object sender, EventArgs e) => OnCurrentTabItemChanged();

		protected override void OnShellItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.OnShellItemsChanged(sender, e);

			SetupMenu();
		}

		protected override void OnShellTabItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnShellTabItemPropertyChanged(sender, e);

			if (e.PropertyName == BaseShellItem.IsEnabledProperty.PropertyName)
			{
				var tab = (ShellTabItem)sender;
				var index = ShellItem.Items.IndexOf(tab);

				var itemCount = ShellItem.Items.Count;
				var maxItems = _bottomView.MaxItemCount;

				if (itemCount > maxItems && index > maxItems - 2)
					return;

				var menuItem = _bottomView.Menu.FindItem(index);
				UpdateShellTabItemEnabled(tab, menuItem);
			}
			else if (e.PropertyName == BaseShellItem.TitleProperty.PropertyName ||
				e.PropertyName == BaseShellItem.IconProperty.PropertyName)
			{
				SetupMenu();
			}
		}

		protected virtual void OnTabReselected(ShellTabItem tab)
		{
		}

		protected virtual void ResetAppearance() => _appearanceTracker.ResetAppearance(_bottomView);

		protected virtual void SetupMenu(IMenu menu, int maxBottomItems, ShellItem shellItem)
		{
			menu.Clear();
			bool showMore = ShellItem.Items.Count > maxBottomItems;

			int end = showMore ? maxBottomItems - 1 : ShellItem.Items.Count;

			var currentIndex = shellItem.Items.IndexOf(CurrentTabItem);

			for (int i = 0; i < end; i++)
			{
				var item = shellItem.Items[i];
				var menuItem = menu.Add(0, i, 0, new Java.Lang.String(item.Title));
				SetMenuItemIcon(menuItem, item.Icon);
				UpdateShellTabItemEnabled(item, menuItem);
				if (item == CurrentTabItem)
				{
					menuItem.SetChecked(true);
				}
			}

			if (showMore)
			{
				var menuItem = menu.Add(0, MoreTabId, 0, new Java.Lang.String("More"));
				menuItem.SetIcon(Resource.Drawable.abc_ic_menu_overflow_material);
				if (currentIndex >= maxBottomItems - 1)
					menuItem.SetChecked(true);
			}

			_bottomView.Visibility = end == 1 ? ViewStates.Gone : ViewStates.Visible;

			_bottomView.SetShiftMode(false, false);
		}

		protected virtual void UpdateShellTabItemEnabled(ShellTabItem tab, IMenuItem menuItem)
		{
			bool tabEnabled = tab.IsEnabled;
			if (menuItem.IsEnabled != tabEnabled)
				menuItem.SetEnabled(tabEnabled);
		}

		private void OnDisplayedPagePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Shell.TabBarVisibleProperty.PropertyName)
				UpdateTabBarVisibility();
		}

		private async void SetImage(ImageView image, ImageSource source)
		{
			image.SetImageDrawable(await Context.GetFormsDrawable(source));
		}

		private async void SetMenuItemIcon(IMenuItem menuItem, ImageSource source)
		{
			if (source == null)
				return;
			var drawable = await Context.GetFormsDrawable(source);
			menuItem.SetIcon(drawable);
		}

		private void SetupMenu() => SetupMenu(_bottomView.Menu, _bottomView.MaxItemCount, ShellItem);

		private void UpdateTabBarVisibility()
		{
			if (DisplayedPage == null)
				return;

			bool visible = Shell.GetTabBarVisible(DisplayedPage);
			_bottomView.Visibility = (visible) ? ViewStates.Visible : ViewStates.Gone;
		}
	}
}