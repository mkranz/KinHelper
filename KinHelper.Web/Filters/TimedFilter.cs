using System.Diagnostics;
using System.Web.Mvc;

namespace KinHelper.Web.Filters
{
    public class Timed : ActionFilterAttribute
    {
        private Stopwatch _stopwatch = new Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Start();
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();

            filterContext.Controller.TempData["TimeElapsedMilliseconds"] = _stopwatch.ElapsedMilliseconds;
            base.OnActionExecuted(filterContext);
        }
    }
}