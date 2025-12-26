using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Services;

public class HapticService : IHapticService
{
    public void PerformClick()
    {
        if (SettingsViewModel.GetEnableHapticFeedback())
        {
            try
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Haptic feedback error: {ex.Message}");
            }
        }
    }

    public void PerformSelection()
    {
        if (SettingsViewModel.GetEnableHapticFeedback())
        {
            try
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Haptic feedback error: {ex.Message}");
            }
        }
    }
}
