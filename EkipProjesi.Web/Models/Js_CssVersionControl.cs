using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Caching;
using System.Web.Mvc;

namespace EkipProjesi.Web.Models
{
    public class Js_CssVersionControl
    {
    }

    public static class EnumExtensions
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date,
                culture.DateTimeFormat.CalendarWeekRule,
                culture.DateTimeFormat.FirstDayOfWeek);
        }

        public static string GetEnumDisplayNameByIndex(int index, Type enumType)
        {
            string display = "";
            try
            {
                var enumValue = Enum.ToObject(enumType, index);
                display = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return display;
        }


        static string GetEnumDisplayNameFound(Enum enumValue)
        {
            string display = "";

            try
            {
                display = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return display;
        }
        public static string GetEnumDisplayNameModel(Enum enumValue)
        {
            return GetEnumDisplayNameFound(enumValue);
        }
        static string GetEnumDisplayDescFound(Enum enumValue)
        {
            string display = "";

            try
            {
                display = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetDescription();
            }
            catch
            {

            }
            return display;
        }
        public static string GetEnumDisplayyDescModel(Enum enumValue)
        {
            return GetEnumDisplayDescFound(enumValue);
        }
        public static MvcHtmlString GetEnumDisplayName(this HtmlHelper helper, Enum enumValue)
        {
            return MvcHtmlString.Create(GetEnumDisplayNameFound(enumValue));
        }
    }
}