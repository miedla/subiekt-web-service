using klient.localhost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient
{
    class Program
    {
        static void Main(string[] args)
        {
            var ws = new SubiektService1();
            ws.UserAuthValue = new UserAuth() { Username = "ampmedia", Password = "ampmedia" };
            string resp = ws.AddUser(11);

            Console.WriteLine("resp: "+resp);

            Console.ReadKey();
        }
    }
}
