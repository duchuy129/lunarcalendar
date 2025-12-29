using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

public interface ILunarCalculationService
{
    LunarDate ConvertToLunar(DateTime gregorianDate);
    DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false);
    List<LunarDate> GetMonthInfo(int year, int month);
}
