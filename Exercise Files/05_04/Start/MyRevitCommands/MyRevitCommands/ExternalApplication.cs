using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MyRevitCommands
{
    class ExternalApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //Create Ribbon Tab
            application.CreateRibbonTab("My Commands");

            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitCommands.PlaceFamily");

            RibbonPanel panel = application.CreateRibbonPanel("My Commands", "Commands");

            panel.AddItem(button);

            return Result.Succeeded;
        }
    }
}
