namespace KittenApp.Web
{
    using KittenApp.Data;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using System;
    using WebServer;

    public class Launcher
    {
        public static void Main()
        {
            try { 
            var context = new KittenAppContext();

            var server = new WebServer(
                42420,
                new ControllerRouter(),
                new ResourceRouter());
            MvcEngine.Run(server, new KittenAppContext());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
