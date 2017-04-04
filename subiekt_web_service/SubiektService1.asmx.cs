using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace subiekt_web_service
{
    /// <summary>
    /// Opis podsumowujący dla SubiektService1
    /// </summary>
    [WebService(Namespace = "http://subiekt-service-amp.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SubiektService1 : System.Web.Services.WebService
    {
        public UserAuth UserAuthValue { get; set; }

        [WebMethod]
        [SoapHeader("UserAuthValue")]
        public string GetUsersFromPortalGames()
        {
            if (AuthorizeUser(UserAuthValue) == true)
            {
                InsERT.Subiekt sgt = GetSubiekt();
                Utils.GetUsersFromPortalGames(sgt);

                return System.Threading.Thread.CurrentThread.GetApartmentState().ToString();
            }
            else
            {
                return "not validate user: " + UserAuthValue.Username;
            }
        }

        [WebMethod]
        [SoapHeader("UserAuthValue")]
        public string AddUser(int order_id)
        {
            if (AuthorizeUser(UserAuthValue) == true)
            {
                //ExecuteCommand(@"psexec -i 0 psexec -s -i 1 C:\windows\system32\Notepad.exe");
                //ExecuteCommand(@"C:\hello.bat");


                //// the name of the application to launch
                //String applicationName = "notepad.exe";

                //// launch the application
                //ApplicationLoader.PROCESS_INFORMATION procInfo;
                //ApplicationLoader.StartProcessAndBypassUAC(applicationName, "c:/",out procInfo);

                //ExecuteCommand(@"net start subiekt_srv");
                ExecuteCommand(@"c:/PSTools/PsExec.exe \\127.0.0.1 -s -d -i 1 C:\Users\ampmedia\Documents\subiekt_app\bin\Debug\subiekt_sfera_test.exe " + order_id);

                return "ok";
            }
            else
            {
                try
                {
                    return "not validate user: " + UserAuthValue.Username;
                }catch(Exception e)
                {
                    return "error: " + e.ToString() + "sta threading: "+ System.Threading.Thread.CurrentThread.GetApartmentState().ToString();
                }
            }
        }

        private static bool AuthorizeUser(UserAuth user)
        {
            if (user != null)
            {
                if (user.Username.Equals("ampmedia") && user.Password.Equals("ampmedia"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private InsERT.Subiekt GetSubiekt()
        {
            InsERT.GT gt = new InsERT.GT();
            InsERT.Subiekt sgt;
            gt.Produkt = InsERT.ProduktEnum.gtaProduktSubiekt;

            gt.Serwer = ConfigConnection.ServerGt; //"(local)\\INSERTGT";
            gt.Baza = ConfigConnection.BazaGt; //"test3";
            if (ConfigConnection.Uzytkownik != "")
            {
                gt.Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaMieszana; //gtaAutentykacjaMieszana;
                gt.Uzytkownik = ConfigConnection.Uzytkownik;
                gt.UzytkownikHaslo = ConfigConnection.UzytkownikHaslo;
            }
            else
            {
                gt.Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaWindows; //gtaAutentykacjaMieszana;
            }
            gt.Operator = ConfigConnection.OperatorGt; //"Szef";
            gt.OperatorHaslo = ConfigConnection.OperatorGThaslo; //"";

            sgt =
                (InsERT.Subiekt)
                gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasujUzytkownika, (int)InsERT.UruchomEnum.gtaUruchom);

            return sgt;
        }

        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            processInfo.Verb = "runas";
            process = Process.Start(processInfo);

            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }
    }

}
