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
                //try
                //{
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
                                                                     //InsERT.Subiekt sgt = GetSubiekt();
                sgt =
                (InsERT.Subiekt)
                gt.Uruchom((int)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (int)InsERT.UruchomEnum.gtaUruchom);

                if (sgt == null)
                {
                    return "sgt ==null";
                }
                //}
                //catch(Exception e)
                //{
                //    return "expception: " + e.ToString();
                //}
                //bool czyDodalo = Utils.PostUserById(sgt, order_id);
                bool czyDodalo = true;
                if (czyDodalo)
                {
                    return "threading: " + System.Threading.Thread.CurrentThread.GetApartmentState().ToString();
                    //return "user was successfully added to subiekt";
                }
                else
                {
                    return "user already exists in subiekt or error occured";
                }
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
    }

}
