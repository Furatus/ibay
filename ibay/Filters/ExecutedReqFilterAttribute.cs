using Microsoft.AspNetCore.Mvc.Filters;

namespace ibay.Filters
{
    public class ExecutedReqFilterAttribute : ActionFilterAttribute
    {
        //Ici après éxecution ("executed" pour après, "executing" pour avant)
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Une requête a été exécutée.");
            Console.ForegroundColor = foreground;

            base.OnActionExecuted(context);
        }
    }
}