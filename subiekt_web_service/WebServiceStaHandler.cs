using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.Web.SessionState;

namespace subiekt_web_service
{
    //public class WebServiceStaHandler
    //{
    /// <summary>
    /// This handler allows SOAP Web Services to run in STA mode
    /// 
    /// Note: this will NOT work with ASP.NET AJAX services as
    /// these services bypass the handler mapping and perform
    /// all work in a module (RestModule).
    /// </summary>
    public class WebServiceStaHandler :
        System.Web.UI.Page, IHttpAsyncHandler
    {
        /// <summary>
        /// Cache the HttpHandlerFactory lookup
        /// </summary>
        static IHttpHandlerFactory _HttpHandlerFactory = null;
        static object _HandlerFactoryLock = new object();

        protected override void OnInit(EventArgs e)
        {
            var factory = this.GetScriptHandlerFactory();

            IHttpHandler handler =
                factory.GetHandler(
                    this.Context,
                    this.Context.Request.HttpMethod,
                    this.Context.Request.FilePath,
                    this.Context.Request.PhysicalPath);
            handler.ProcessRequest(this.Context);

            // immediately stop the request after we're done processing
            this.Context.ApplicationInstance.CompleteRequest();
        }


        /// <summary>
        /// Loader for the script handler factory
        /// </summary>
        IHttpHandlerFactory GetScriptHandlerFactory()
        {
            if (_HttpHandlerFactory != null)
                return _HttpHandlerFactory;

            IHttpHandlerFactory factory = null;

            lock (_HandlerFactoryLock)
            {
                if (_HttpHandlerFactory != null)
                    return _HttpHandlerFactory;

                try
                {
                    // Try to load the ScriptServiceHandlerFactory
                    // class is internal requires reflection so this requires full trust
                    var assembly = typeof(JavaScriptSerializer).Assembly;
                    var shf = assembly.GetTypes().Where(t => t.Name == "ScriptHandlerFactory").FirstOrDefault();
                    factory = Activator.CreateInstance(shf) as IHttpHandlerFactory;
                }
                catch { }

                // Fallback to just WebService Handler Factory
                if (factory == null)
                    factory = new WebServiceHandlerFactory();

                _HttpHandlerFactory = factory;
            }

            return factory;
        }


        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
        }

        public IAsyncResult BeginProcessRequest(
            HttpContext context, AsyncCallback cb, object extraData)
        {
            return this.AspCompatBeginProcessRequest(context, cb, extraData);
        }


        public void EndProcessRequest(IAsyncResult result)
        {
            this.AspCompatEndProcessRequest(result);
        }
    }

    public class AspCompatWebServiceStaHandlerWithSessionState :
                    WebServiceStaHandler, IRequiresSessionState
    {
    }
    //}
}