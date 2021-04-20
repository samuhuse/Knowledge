using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacPatterns.Model
{
    public interface ILogger
    {
        public void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        private UInt64 _count = 0;
        public void Log(string message)
        {
            _count++;
            Console.Write($"writing message {_count}:");
            Console.WriteLine(message);
        }
    }

    public class EmailLogger : ILogger
    {
        private const string _destinatary = "samuele.mn@hotmail.it";
        public void Log(string message)
        {
            Console.WriteLine($"Email sent to {_destinatary}: {message}");
        }
    }

    public class SmsLogger : ILogger
    {
        private string _phoneNumber;
        public SmsLogger(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
        }
        public void Log(string message)
        {
            Console.WriteLine($"sent sms to {_phoneNumber}: {message}");
        }
    }
}
