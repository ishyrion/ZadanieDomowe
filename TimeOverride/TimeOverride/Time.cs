using System;
using System.Collections.Generic;
using System.Text;

namespace TimeOverride
{
    struct Time : IEquatable<Time>, IComparable<Time>
    {
        private readonly byte hours;
        private readonly byte minutes;
        private readonly byte seconds;
        private readonly ushort miliseconds;

        public byte Hours => hours;
        public byte Minutes => minutes;
        public byte Seconds => seconds;
        public ushort Miliseconds => miliseconds;

        public Time(byte hours, byte minutes = 0, byte seconds = 0, ushort miliseconds = 0)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.miliseconds = 0;

            if (hours >= 24 || minutes >= 60 || seconds >= 60 || miliseconds >= 1000)
                throw new ArgumentOutOfRangeException();
        }

        public Time(Time other)
        {
            hours = other.Hours;
            minutes = other.Minutes;
            seconds = other.Seconds;
            miliseconds = other.Miliseconds;
        }


        public Time(string s)
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
            miliseconds = 0;

            if (s.Length == 8 || s.Length == 12)
                throw new ArgumentException();

            if (Byte.TryParse(s.Substring(0, 2), out hours) == false)
                throw new ArgumentException();
            if (hours >= 24)
                throw new ArgumentOutOfRangeException();

            if (Byte.TryParse(s.Substring(3, 5), out minutes) == false)
                throw new ArgumentException();
            if (minutes >= 60)
                throw new ArgumentOutOfRangeException();

            if (Byte.TryParse(s.Substring(6, 8), out seconds) == false)
                throw new ArgumentException();
            if (seconds >= 60)
                throw new ArgumentOutOfRangeException();

            if (s.Length == 12 && UInt16.TryParse(s.Substring(9, 12), out miliseconds) == false)
                throw new ArgumentException();
        }

        public override string ToString()
        {
            return $"{hours / 10}{hours % 10}:{minutes / 10}{minutes % 10}:{seconds / 10}{seconds % 10}";
        }

        public bool Equals(Time other)
        {
            if (hours != other.Hours || minutes != other.Minutes || seconds != other.Seconds || miliseconds != other.Miliseconds)
                return false;

            return true;
        }

        public int CompareTo(Time other)
        {
            if (this.Equals(other))
                return 0;
            if (hours.Equals(other.Hours) == false)
                return hours.CompareTo(other.Hours);
            if (minutes.Equals(other.Minutes) == false)
                return minutes.CompareTo(other.Minutes);
            if (seconds.Equals(other.Seconds) == false)
                return seconds.CompareTo(other.Seconds);

            return miliseconds.CompareTo(other.Miliseconds);
        }


        public static bool operator ==(Time x, Time y) => x.Equals(y);
        public static bool operator !=(Time x, Time y) => x.Equals(y) == false;
        public static bool operator >(Time x, Time y) => x.CompareTo(y) == 1;
        public static bool operator <(Time x, Time y) => x.CompareTo(y) == -1;
        public static bool operator >=(Time x, Time y) => x.CompareTo(y) >= 0;
        public static bool operator <=(Time x, Time y) => x.CompareTo(y) <= 0;
        public Time Plus(TimePeriod other)
        {
            ushort mil = (ushort)(miliseconds + other.Miliseconds);
            long sec = seconds + other.Seconds + mil / 1000;
            mil %= 1000;
            long min = minutes + sec / 60;
            sec %= 60;
            long hou = hours + min / 60;
            min %= 60;
            hou %= 24;

            return new Time((byte)hou, (byte)min, (byte)sec, mil);
        }


        public static Time Plus(Time x, TimePeriod y) => x.Plus(y);
        public static Time operator +(Time x, TimePeriod y) => x.Plus(y);


    }
}
