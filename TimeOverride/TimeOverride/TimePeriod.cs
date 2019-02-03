using System;
using System.Collections.Generic;
using System.Text;

namespace TimeOverride
{
    struct TimePeriod : IEquatable<TimePeriod>, IComparable<TimePeriod>
    {
        private readonly long seconds;
        private readonly ushort miliseconds;

        public long Seconds => seconds;
        public ushort Miliseconds => miliseconds;

        public TimePeriod(long hours, byte minutes, byte seconds = 0, ushort miliseconds = 0)
        {
            this.seconds = hours * 60 * 60 + (long)minutes * 60 + (long)seconds;
            this.miliseconds = miliseconds;


            if (hours < 0 || minutes < 0 || seconds < 0 || miliseconds < 0)
                throw new ArgumentOutOfRangeException();

            if (minutes >= 60 || seconds >= 60 || miliseconds >= 1000)
                throw new ArgumentOutOfRangeException();
        }

        public TimePeriod(long seconds, ushort miliseconds = 0)
        {
            if (seconds < 0 || miliseconds < 0)
                throw new ArgumentOutOfRangeException();

            this.seconds = seconds;
            this.miliseconds = miliseconds;
        }



        public TimePeriod(string s)
        {
            seconds = 0;
            miliseconds = 0;
            long hou;
            byte min, sec; ;

            if (s.Length == 10 || s.Length == 14)
                throw new ArgumentException();

            if (Int64.TryParse(s.Substring(0, 4), out hou) == false)
                throw new ArgumentException();
            if (hou < 0)
                throw new ArgumentOutOfRangeException();

            if (Byte.TryParse(s.Substring(5, 7), out min) == false)
                throw new ArgumentException();
            if (min >= 60 || min < 0)
                throw new ArgumentOutOfRangeException();

            if (Byte.TryParse(s.Substring(8, 10), out sec) == false)
                throw new ArgumentException();
            if (sec >= 60 || sec < 0)
                throw new ArgumentOutOfRangeException();

            if (s.Length == 14 && UInt16.TryParse(s.Substring(11, 14), out miliseconds) == false)
                throw new ArgumentException();
            if (miliseconds < 0)
                throw new ArgumentOutOfRangeException();

            seconds = (long)sec + (long)min * 60 + (long)hou * 60 * 60;
        }



        public TimePeriod(Time x1, Time x2)
        {
            seconds = ((long)x2.Hours - (long)x1.Hours) * 60 * 60 + ((long)x2.Minutes - (long)x1.Minutes) * 60 +
                      ((long)x2.Seconds - (long)x1.Seconds);
            if (x1.Miliseconds > x2.Miliseconds)
            {
                miliseconds = (ushort)(1000 + x2.Miliseconds - x1.Miliseconds);
                seconds--;
            }
            else
                miliseconds = (ushort)(x2.Miliseconds - x1.Miliseconds);
        }


        public override string ToString()
        {
            long sec = seconds % 60;
            long min = (seconds / 60) % 60;
            long hou = seconds / (60 * 60);

            return $"{hou / 100}{(hou / 10) % 10}{hou % 10}:{min / 10}{min % 10}:{sec / 10}{sec % 10}";
        }

        public bool Equals(TimePeriod other)
        {
            if (seconds != other.Seconds || miliseconds != other.Miliseconds)
                return false;

            return true;
        }

        public int CompareTo(TimePeriod other)
        {
            if (this.Equals(other))
                return 0;
            if (seconds.Equals(other.Seconds) == false)
                return seconds.CompareTo(other.Seconds);

            return miliseconds.CompareTo(other.Miliseconds);
        }


        public static bool operator ==(TimePeriod x, TimePeriod y) => x.Equals(y);
        public static bool operator !=(TimePeriod x, TimePeriod y) => x.Equals(y) == false;
        public static bool operator >(TimePeriod x, TimePeriod y) => x.CompareTo(y) == 1;
        public static bool operator <(TimePeriod x, TimePeriod y) => x.CompareTo(y) == -1;
        public static bool operator >=(TimePeriod x, TimePeriod y) => x.CompareTo(y) >= 0;
        public static bool operator <=(TimePeriod x, TimePeriod y) => x.CompareTo(y) <= 0;

        public TimePeriod Plus(TimePeriod other) => new TimePeriod(seconds + other.Seconds + (miliseconds + other.Miliseconds) / 1000,
                                                    (ushort)((miliseconds + other.Miliseconds) % 1000));
        public static TimePeriod Plus(TimePeriod x, TimePeriod y) => x.Plus(y);
        public static TimePeriod operator +(TimePeriod x, TimePeriod y) => x.Plus(y);

    }
}
