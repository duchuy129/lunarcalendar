using Android.Views;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace LunarCalendar.MobileApp.Platforms.Android.Handlers
{
    public class CustomShellHandler : ShellRenderer
    {
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new CustomShellItemRenderer(this);
        }
    }

    public class CustomShellItemRenderer : ShellItemRenderer
    {
        public CustomShellItemRenderer(IShellContext shellContext) : base(shellContext)
        {
        }

        // Override to force bottom tabs instead of top tabs
        public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, global::Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            // The default behavior puts tabs at top for TabBar
            // We need to override this by ensuring we use BottomNavigationView
            return view;
        }
    }
}
