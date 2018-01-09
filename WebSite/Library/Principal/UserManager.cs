using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EPA.Project.WebSite.Library.Principal
{
    public class UserManager
    {
        /// <summary>
        /// Returns the User from the Context.User.Identity by decrypting the forms auth ticket and returning the user object.
        /// </summary>
        public static WebSiteUser User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var formI = (FormsIdentity)HttpContext.Current.User.Identity;
                    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                    WebSiteUser data = ser.Deserialize<WebSiteUser>(formI.Ticket.UserData);
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}