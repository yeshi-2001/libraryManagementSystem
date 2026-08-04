using System;
using System.Web;
using System.Web.UI;

namespace libraryManagementSystem
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js",
                DebugPath = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.js"
            });
        }
    }
}