using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Data;

public static class HolidaySeeder
{
    public static List<Holiday> GetVietnameseLunarHolidays()
    {
        return new List<Holiday>
        {
            // MAJOR HOLIDAYS (Red)
            new Holiday
            {
                Id = 1,
                Name = "Tết Nguyên Đán",
                Description = "Lunar New Year - The most important Vietnamese holiday",
                LunarMonth = 1,
                LunarDay = 1,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },
            new Holiday
            {
                Id = 2,
                Name = "Tết Nguyên Đán (Day 2)",
                Description = "Lunar New Year Day 2",
                LunarMonth = 1,
                LunarDay = 2,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },
            new Holiday
            {
                Id = 3,
                Name = "Tết Nguyên Đán (Day 3)",
                Description = "Lunar New Year Day 3",
                LunarMonth = 1,
                LunarDay = 3,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },
            new Holiday
            {
                Id = 4,
                Name = "Giỗ Tổ Hùng Vương",
                Description = "Hung Kings' Festival - Commemoration of the Hung Kings",
                LunarMonth = 3,
                LunarDay = 10,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },

            // GREGORIAN-BASED MAJOR HOLIDAYS
            new Holiday
            {
                Id = 5,
                Name = "Tết Dương Lịch",
                Description = "New Year's Day",
                GregorianMonth = 1,
                GregorianDay = 1,
                LunarMonth = 0,
                LunarDay = 0,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },
            new Holiday
            {
                Id = 6,
                Name = "Ngày Giải Phóng Miền Nam",
                Description = "Reunification Day",
                GregorianMonth = 4,
                GregorianDay = 30,
                LunarMonth = 0,
                LunarDay = 0,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },
            new Holiday
            {
                Id = 7,
                Name = "Ngày Quốc Tế Lao Động",
                Description = "International Labor Day",
                GregorianMonth = 5,
                GregorianDay = 1,
                LunarMonth = 0,
                LunarDay = 0,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },
            new Holiday
            {
                Id = 8,
                Name = "Quốc Khánh",
                Description = "National Day - Independence Day",
                GregorianMonth = 9,
                GregorianDay = 2,
                LunarMonth = 0,
                LunarDay = 0,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = true
            },

            // TRADITIONAL FESTIVALS (Gold/Yellow)
            new Holiday
            {
                Id = 9,
                Name = "Tết Nguyên Tiêu",
                Description = "Lantern Festival - First full moon of the year",
                LunarMonth = 1,
                LunarDay = 15,
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FFD700",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 10,
                Name = "Tết Hàn Thực",
                Description = "Cold Food Festival",
                LunarMonth = 3,
                LunarDay = 3,
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FFD700",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 11,
                Name = "Tết Đoan Ngọ",
                Description = "Dragon Boat Festival",
                LunarMonth = 5,
                LunarDay = 5,
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FFD700",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 12,
                Name = "Vu Lan",
                Description = "Ullambana Festival - Wandering Souls' Day",
                LunarMonth = 7,
                LunarDay = 15,
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FFD700",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 13,
                Name = "Tết Trung Thu",
                Description = "Mid-Autumn Festival - Children's Festival",
                LunarMonth = 8,
                LunarDay = 15,
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FFD700",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 14,
                Name = "Tết Trùng Cửu",
                Description = "Double Ninth Festival",
                LunarMonth = 9,
                LunarDay = 9,
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FFD700",
                IsPublicHoliday = false
            },

            // SEASONAL CELEBRATIONS (Green)
            new Holiday
            {
                Id = 15,
                Name = "Rằm Tháng Giêng",
                Description = "First full moon of the year",
                LunarMonth = 1,
                LunarDay = 15,
                Type = HolidayType.SeasonalCelebration,
                ColorHex = "#32CD32",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 16,
                Name = "Tết Thanh Minh",
                Description = "Tomb Sweeping Day",
                LunarMonth = 3,
                LunarDay = 3,
                Type = HolidayType.SeasonalCelebration,
                ColorHex = "#32CD32",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 17,
                Name = "Rằm Tháng Bảy",
                Description = "Ghost Festival - 15th day of 7th lunar month",
                LunarMonth = 7,
                LunarDay = 15,
                Type = HolidayType.SeasonalCelebration,
                ColorHex = "#32CD32",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 18,
                Name = "Tết Ông Công Ông Táo",
                Description = "Kitchen Gods' Day",
                LunarMonth = 12,
                LunarDay = 23,
                Type = HolidayType.SeasonalCelebration,
                ColorHex = "#32CD32",
                IsPublicHoliday = false
            },
            new Holiday
            {
                Id = 19,
                Name = "Giao Thừa",
                Description = "New Year's Eve",
                LunarMonth = 12,
                LunarDay = 30,
                Type = HolidayType.MajorHoliday,
                ColorHex = "#DC143C",
                IsPublicHoliday = false
            }
        };
    }
}
