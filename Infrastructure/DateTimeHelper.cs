﻿using System;
using System.ComponentModel;

namespace Infrastructure
{
    public static class DateTimeHelper
    {
        public static readonly string yMdHm = "yyyy-MM-dd HH:mm";//转换为24小时制,例: 2018-06-27 15:24
        public static readonly string yMdhm = "yyyy-MM-dd hh:mm";//转换为12小时制,例: 2018-06-27 03:24

        /// <summary>
        /// 时间转义
        /// </summary>
        /// <param name="datetime">时间：2018-09-04 0630：30.000</param>
        /// <param name="yyyyMMddHHmm">是否精确到时分秒</param>
        /// <returns></returns>
        public static string GetDateDesc(this DateTime? dateSpan, bool yyyyMMddHHmm = false)
        {
            if (!dateSpan.HasValue)
            {
                return string.Empty;
            }
            string rtn;
            var datetime = dateSpan.Value;
            var now = DateTime.Now;
            var today = now.Date;  //2018-9-4 0:00:00 今天凌晨
            var yesterday = now.AddDays(-1).Date;  //2018-9-3 0:00:00  昨天凌晨
            var lastWeek = now.AddDays(-7);  //最近一周
            var lastYear = now.AddYears(-1); //最近一年
            if (datetime > today)
            {
                var min1 = now.AddMinutes(-1);       //    1分钟前
                var min2 = now.AddMinutes(-2);       //    2分钟前
                var min5 = now.AddMinutes(-5);       //    5分钟前
                var min10 = now.AddMinutes(-10);     //    10分钟前
                var min20 = now.AddMinutes(-20);     //    20分钟前
                var min30 = now.AddMinutes(-30);     //    30分钟前
                var hour1 = now.AddHours(-1);      //    1小时前
                var hour2 = now.AddHours(-2);      //    2小时前
                var hour3 = now.AddHours(-3);      //    3小时前
                var hour4 = now.AddHours(-4);      //    4小时前
                var hour5 = now.AddHours(-5);      //    5小时前
                var hour6 = now.AddHours(-6);      //    6小时前
                var hour7 = now.AddHours(-7);      //    7小时前
                var hour8 = now.AddHours(-8);      //    8小时前
                var hour9 = now.AddHours(-9);      //    9小时前
                var hour10 = now.AddHours(-10);    //    10小时前
                var hour11 = now.AddHours(-11);    //    11小时前
                var hour12 = now.AddHours(-12);    //    12小时前
                var hour13 = now.AddHours(-13);    //    13小时前
                var hour14 = now.AddHours(-14);    //    14小时前
                var hour15 = now.AddHours(-15);    //    15小时前
                var hour16 = now.AddHours(-16);    //    16小时前
                var hour17 = now.AddHours(-17);    //    17小时前
                var hour18 = now.AddHours(-18);    //    18小时前
                var hour19 = now.AddHours(-19);    //    19小时前
                var hour20 = now.AddHours(-20);    //    20小时前
                var hour21 = now.AddHours(-21);    //    21小时前
                var hour22 = now.AddHours(-22);    //    22小时前
                var hour23 = now.AddHours(-23);    //    23小时前
                if (datetime >= min1 && datetime < now)
                {
                    rtn = "刚刚";
                }
                else if (datetime >= min2 && datetime < min1)
                {
                    rtn = "1分钟前";
                }
                else if (datetime >= min5 && datetime < min2)
                {
                    rtn = "2分钟前";
                }
                else if (datetime >= min10 && datetime < min5)
                {
                    rtn = "5分钟前";
                }
                else if (datetime >= min20 && datetime < min10)
                {
                    rtn = "10分钟前";
                }
                else if (datetime >= min30 && datetime < min20)
                {
                    rtn = "20分钟前";
                }
                else if (datetime >= hour1 && datetime < min30)
                {
                    rtn = "30分钟前";
                }
                else if (datetime >= hour2 && datetime < hour1)
                {
                    rtn = "1小时前";
                }
                else if (datetime >= hour3 && datetime < hour2)
                {
                    rtn = "2小时前";
                }
                else if (datetime >= hour4 && datetime < hour3)
                {
                    rtn = "3小时前";
                }
                else if (datetime >= hour5 && datetime < hour4)
                {
                    rtn = "4小时前";
                }
                else if (datetime >= hour6 && datetime < hour5)
                {
                    rtn = "5小时前";
                }
                else if (datetime >= hour7 && datetime < hour6)
                {
                    rtn = "6小时前";
                }
                else if (datetime >= hour8 && datetime < hour7)
                {
                    rtn = "8小时前";
                }
                else if (datetime >= hour10 && datetime < hour9)
                {
                    rtn = "9小时前";
                }
                else if (datetime >= hour11 && datetime < hour10)
                {
                    rtn = "10小时前";
                }
                else if (datetime >= hour12 && datetime < hour11)
                {
                    rtn = "11小时前";
                }
                else if (datetime >= hour13 && datetime < hour12)
                {
                    rtn = "12小时前";
                }
                else if (datetime >= hour14 && datetime < hour13)
                {
                    rtn = "13小时前";
                }
                else if (datetime >= hour15 && datetime < hour14)
                {
                    rtn = "14小时前";
                }
                else if (datetime >= hour16 && datetime < hour15)
                {
                    rtn = "15小时前";
                }
                else if (datetime >= hour17 && datetime < hour16)
                {
                    rtn = "16小时前";
                }
                else if (datetime >= hour18 && datetime < hour17)
                {
                    rtn = "17小时前";
                }
                else if (datetime >= hour19 && datetime < hour18)
                {
                    rtn = "18小时前";
                }
                else if (datetime >= hour20 && datetime < hour19)
                {
                    rtn = "19小时前";
                }
                else if (datetime >= hour21 && datetime < hour20)
                {
                    rtn = "20小时前";
                }
                else if (datetime >= hour22 && datetime < hour21)
                {
                    rtn = "21小时前";
                }
                else if (datetime >= hour23 && datetime < hour22)
                {
                    rtn = "22小时前";
                }
                else
                {
                    rtn = "23小时前";
                }
            }
            else if (datetime <= today && datetime > yesterday)
            {
                rtn = "昨天";
            }
            else if (datetime <= yesterday && datetime > lastYear)
            {
                if (yyyyMMddHHmm)
                {
                    //8月2日 20:23
                    var date = datetime.GetDateTimeFormats('M')[0].ToString();
                    var time = datetime.GetDateTimeFormats('t')[0].ToString();
                    rtn = string.Format("{0} {1}", date, time);
                }
                else
                {
                    //8月2日
                    rtn = datetime.GetDateTimeFormats('M')[0].ToString();
                }
            }
            else
            {
                if (yyyyMMddHHmm)
                {
                    //2016年8月2日 20:23
                    rtn = datetime.ToString("f");
                }
                else
                {
                    //2016年8月2日
                    rtn = datetime.ToString("D");
                }
            }
            return rtn;
        }

        /// <summary>
        /// 获取聊天框时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetChatDetailDateTime(this DateTime datetime)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var yesterday = now.AddDays(-1).Date;
            if (datetime > today)
            {
                if (datetime.Hour >= 0 && datetime.Hour < 6)
                {
                    //凌晨 12:10
                    return string.Format("凌晨 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
                else if (datetime.Hour >= 6 && datetime.Hour < 9)
                {
                    return string.Format("早上 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
                else if (datetime.Hour >= 9 && datetime.Hour < 12)
                {
                    return string.Format("上午 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
                else if (datetime.Hour >= 12 && datetime.Hour < 13)
                {
                    return string.Format("中午 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
                else if (datetime.Hour >= 13 && datetime.Hour < 14)
                {
                    return string.Format("中午 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
                else if (datetime.Hour >= 14 && datetime.Hour < 19)
                {
                    return string.Format("下午 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
                else
                {
                    return string.Format("晚上 {0}", GetMinute(datetime.Hour, datetime.Minute));
                }
            }
            else if (datetime > yesterday && datetime < today)
            {
                return string.Format("昨天 {0}", GetMinute(datetime.Hour, datetime.Minute));
            }
            else
            {
                return datetime.ToString("f");
            }
        }

        private static string GetMinute(int hour, int minute)
        {
            string hourDesc;
            string minuteDesc;
            if (hour < 10)
            {
                hourDesc = string.Format("0{0}", hour);
            }
            else
            {
                if (hour >= 13)
                {
                    int hour1 = hour - 12;
                    if (hour1 < 10)
                    {
                        hourDesc = string.Format("0{0}", hour1);
                    }
                    else
                    {
                        hourDesc = hour1.ToString();
                    }
                }
                else
                {
                    hourDesc = hour.ToString();
                }
            }

            if (minute < 10)
            {
                minuteDesc = string.Format("0{0}", minute);
            }
            else
            {
                minuteDesc = minute.ToString();
            }
            return string.Format("{0}:{1}", hourDesc, minuteDesc);
        }

        /// <summary>
        /// 获取在线状态描述
        /// </summary>
        public static string GetOnlineDesc(this DateTime? datetime, bool isonline, string defaultValue = "")
        {
            if (isonline)
            {
                return "当前在线";
            }
            if (datetime == null || !datetime.HasValue)
            {
                return defaultValue;
            }
            var second = DateTime.Now.Subtract(datetime.Value).TotalSeconds;
            if (0 < second && second < 600)
            {
                return "当前在线";
            }
            else if (600 < second && second < 1200)
            {
                return "10分钟前在线";
            }
            else if (1200 < second && second < 1800)
            {
                return "20分钟前在线";
            }
            else if (1800 < second && second < 2400)
            {
                return "30分钟前在线";
            }
            else if (2400 < second && second < 3000)
            {
                return "40分钟前在线";
            }
            else if (3000 < second && second < 3600)
            {
                return "50分钟前在线";
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 根据出生日期计算年龄
        /// </summary>
        public static int GetAgeByBirthdate(this DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        public static string GetAgeYear(this DateTime? birthdate)
        {
            if (!birthdate.HasValue)
            {
                return string.Empty;
            }
            int year = birthdate.Value.Year;
            if (year >= 2010)
            {
                return "10后";
            }
            else if (year >= 2005 && year < 2010)
            {
                return "05后";
            }
            else if (year >= 2000 && year < 2005)
            {
                return "00后";
            }
            else if (year >= 1995 && year < 2000)
            {
                return "95后";
            }
            else if (year >= 1990 && year < 1995)
            {
                return "90后";
            }
            else if (year >= 1985 && year < 1990)
            {
                return "85后";
            }
            else if (year >= 1980 && year < 1985)
            {
                return "80后";
            }
            else if (year >= 1975 && year < 1980)
            {
                return "75后";
            }
            else if (year >= 1970 && year < 1975)
            {
                return "70后";
            }
            else if (year >= 1965 && year < 1970)
            {
                return "65后";
            }
            else if (year >= 1960 && year < 1965)
            {
                return "60后";
            }
            else if (year >= 1955 && year < 1960)
            {
                return "55后";
            }
            else if (year >= 1950 && year < 1955)
            {
                return "50后";
            }
            else if (year >= 1945 && year < 1950)
            {
                return "45后";
            }
            else if (year >= 1940 && year < 1945)
            {
                return "40后";
            }
            else
            {
                return "神秘年龄";
            }
        }

        public static string GetMonthDesc(this int month)
        {
            switch (month)
            {
                case 1:
                    return "一月";
                case 2:
                    return "二月";
                case 3:
                    return "三月";
                case 4:
                    return "四月";
                case 5:
                    return "五月";
                case 6:
                    return "六月";
                case 7:
                    return "七月";
                case 8:
                    return "八月";
                case 9:
                    return "九月";
                case 10:
                    return "十月";
                case 11:
                    return "十一月";
                case 12:
                    return "十二月";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 根据出生日期获得星座信息
        /// </summary>
        public static string GetConstellation(this DateTime? birthdate)
        {
            if (!birthdate.HasValue)
            {
                return string.Empty;
            }
            var rtn = Constellation.Acrab;
            float birthdayF = birthdate.Value.Month == 1 && birthdate.Value.Day < 20 ?
                13 + birthdate.Value.Day / 100f : birthdate.Value.Month + birthdate.Value.Day / 100f;

            float[] bound = { 1.20F, 2.20F, 3.21F, 4.21F, 5.21F, 6.22F, 7.23F, 8.23F, 9.23F, 10.23F, 11.21F, 12.22F, 13.20F };

            var constellations = new Constellation[12];
            for (int i = 0; i < constellations.Length; i++)
            {
                constellations[i] = (Constellation)(i + 1);
            }

            for (int i = 0; i < bound.Length - 1; i++)
            {
                float b = bound[i];
                float nextB = bound[i + 1];
                if (birthdayF >= b && birthdayF < nextB)
                {
                    rtn = constellations[i];
                }
            }

            return rtn.ToDescription();
        }

        private enum Constellation
        {
            [Description("水瓶座")]
            Aquarius = 1,//1.20 - 2.18

            [Description("双鱼座")]
            Pisces = 2,//2.19 - 3.20

            [Description("白羊座")]
            Aries = 3,//3.21 - 4.19          

            [Description("金牛座")]
            Taurus = 4, //4.20 - 5.20

            [Description("双子座")]
            Gemini = 5,//5.21 - 6.21

            [Description("巨蟹座")]
            Cancer = 6,//6.22 - 7.22

            [Description("狮子座")]
            Leo = 7,// 7.23 - 8.22

            [Description("处女座")]
            Virgo = 8,//8.23 - 9.22

            [Description("天秤座")]
            Libra = 9,//9.23 - 10.23

            [Description("天蝎座")]
            Acrab = 10,//10.24 - 11.22

            [Description("射手座")]
            Sagittarius = 11,//11.23 - 12.21

            [Description("摩羯座")]
            Capricornus = 12,//12.22 - 1.19
        }
    }
}
