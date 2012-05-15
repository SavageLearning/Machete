using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Machete.Web.Helpers
{
    //Take from stackoverflow.com discussion
    //http://stackoverflow.com/questions/4649795/hiding-column-in-table-based-on-role-in-mvc
    //
    public static class IsInRoleHelper
    {
        public static bool IsInRole(this HtmlHelper instance, params string[] roles)
        {
            var user = instance.ViewContext.HttpContext.User;
            foreach (var role in roles)
            {
                if (user.IsInRole(role))
                    return true;
            }
            return false;
        }
    }
    // TODO: RoleGroupHelper relies on magic strings
    public static class RoleGroupHelper
    {
        public static string[] Role_AMPCU(this HtmlHelper html) { return new[]{"Administrator", "Manager", "PhoneDesk", "Check-in", "User"}; }
        public static string[] Role_AMPU(this HtmlHelper html) { return new[]{"Administrator", "Manager", "PhoneDesk", "User"}; }
        public static string[] Role_AMP(this HtmlHelper html) { return new[]{"Administrator", "Manager", "PhoneDesk"}; }
        public static string[] Role_AM(this HtmlHelper html) { return new[] { "Administrator", "Manager" }; }
        public static string[] Role_A(this HtmlHelper html) {return new[]{"Administrator"}; }
        public static string[] Role_T(this HtmlHelper html) { return new[] { "Teacher" }; }
    }  
}