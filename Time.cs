using System.ComponentModel;
using System;

namespace Plugins;

public class TimePlugin
{
  [Description("Get the current date and time")]
  public static DateTime Time()
  {
    Console.WriteLine($"[Time called at {DateTime.UtcNow}]");
    return DateTime.UtcNow;
  }

  [Description("get the current day of the week")]
  public static System.DayOfWeek DayOfWeek()
  {
    Console.WriteLine($"[DayOfWeek called at {DateTime.UtcNow}]");
    return DateTime.UtcNow.DayOfWeek;
  }
}