using System.Collections.Generic;
using Machete.Test.Selenium.View;
using OpenQA.Selenium;

namespace Machete.Test.UINav
{
    class Elem
    {
        protected static sharedUI ui { get; set; }
        public string id {get; set;}

        public void Click()
        {
            ui.WaitThenClickElement(By.Id(id));
        }
    }
    class TabBar : Elem
    {
        public string recType;
        public ListTab ListTab { get; set; }
        public CreateTab CreateTab { get; set; }
        public List<Tab> editTabs { get; set; }
    }

    class Tab : Elem
    {

    }

    class ListTab : Tab
    {
        public ListTab(string recType)
        {
            id = recType + "ListTab";
        }
    }

    class CreateTab : Tab
    {
        public CreateTab(string recType)
        {
            id = recType + "CreateTab";
        }
    }
    class Emp : TabBar
    {
        public Emp(sharedUI UI)
        {
            ui = UI;
            id = "employerTabs";
            recType = "employer";
            ListTab = new ListTab(recType);
            CreateTab = new CreateTab(recType);
        }
    }

    class WO : TabBar
    {
    }

    class WA : TabBar
    {
    }


}
