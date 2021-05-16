using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures.Exceptions
{
    public class HandlingExceptions
    {
        void ThrowRandomException()
        {
            int random = new Random().Next(1,4);

            switch (random)
            {
                case 1: throw new InvalidOperationException();
                case 2: throw new WebException("", WebExceptionStatus.ProtocolError);
                case 3: throw new WebException("", WebExceptionStatus.Timeout);
                default: throw new System.Exception();
            }
        }

        [Test]
        public void TryHandle()
        {
            try
            {
                ThrowRandomException();
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError)
            {
                Console.WriteLine("Web Exception, Protocol Error");
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                Console.WriteLine("Web Exception, Timeout Error");
            }
            catch (InvalidOperationException ex) 
            {
                Console.WriteLine("InvalidOperationException");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception");
            }
            finally
            {
                Console.WriteLine("finally block");
            }
        }
    }
}
