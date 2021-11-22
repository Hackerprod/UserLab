using System;

namespace SKYNET
{
    public class modCommon
    {
        public static ulong GenerateID()
        {
            Random r = new Random();
            return (ulong)r.Next(1000, 9999999);
        }
    }
}