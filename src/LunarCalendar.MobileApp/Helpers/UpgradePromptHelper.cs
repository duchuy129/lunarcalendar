namespace LunarCalendar.MobileApp.Helpers;

public static class UpgradePromptHelper
{
    public static async Task<bool> ShowUpgradePromptAsync(
        string title = "Sign Up Required",
        string message = "This feature requires an account. Create one to continue?")
    {
        var result = await Shell.Current.DisplayActionSheet(
            message,
            "Cancel",
            null,
            "Sign Up",
            "Sign In");

        if (result == "Sign Up")
        {
            await Shell.Current.GoToAsync("//register");
            return true;
        }
        else if (result == "Sign In")
        {
            await Shell.Current.GoToAsync("//login");
            return true;
        }

        return false;
    }

    public static async Task ShowFeatureLockedAsync(string featureName)
    {
        await ShowUpgradePromptAsync(
            "Feature Locked",
            $"{featureName} is available for registered users. Sign up to unlock!");
    }
}
