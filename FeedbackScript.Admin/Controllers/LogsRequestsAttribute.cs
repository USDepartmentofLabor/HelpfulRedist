using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using FeedbackScript.Admin.Models;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Principal;
using System.Text;


namespace FeedbackScript.Admin.Controllers
{
	public class LogRequestAttribute : ActionFilterAttribute, IActionFilter
	{
        private static FeedbackreportsDataContext DataStore = new FeedbackreportsDataContext();

        //Called before an action method executes.
        //Important: Currently it is not used
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
		{
            string parameterInfo = string.Empty;
            StringBuilder CompleteParam = new StringBuilder();

			ThreadPool.QueueUserWorkItem(delegate
			{
                try
                {
                    //Append all the parameters to stringbuilder variable
                    if (filterContext.ActionParameters != null)
                    {
                        foreach (KeyValuePair<string, object> parameter in filterContext.ActionParameters)
                        {
                            parameterInfo = string.Format("Parameter name: {0} – Parameter value: {1}", parameter.Key, parameter.Value == null ? "null" : parameter.Value);
                            CompleteParam.Append(parameterInfo + Environment.NewLine);
                        }
                    }
                    //Log the parameters, action, user name and current date time into Audit_Logs table
                    Audit_Log log = new Audit_Log();
                    log.Event_Result = true;
                    log.Log_Datetime = System.DateTime.Now;
                    log.User_Action = filterContext.Controller.GetType().Name + "\\" + filterContext.ActionDescriptor.ActionName;
                    log.User_identity = filterContext.RequestContext.HttpContext.User.Identity.Name;
                    log.User_Parameters = CompleteParam.ToString();
                    DataStore.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["FeedbackScriptConnectionString"].ConnectionString;
                    DataStore.Audit_Logs.InsertOnSubmit(log);
                    DataStore.SubmitChanges();
                }
                catch
                {
                    return;
                }
				finally { }
			});
		}

        public class HandleErrorAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                if (filterContext.Exception != null)
                {

                }
            }
        }

	}
}
