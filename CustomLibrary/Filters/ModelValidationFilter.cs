using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomLibrary.Helper;

namespace CustomLibrary.Filters
{
    public class ModelValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var exceptionMessages = context.ModelState.Values.SelectMany(e => e.Errors.Select(x => x.ErrorMessage)); ;

                //throw new BadHttpRequestException($"Model Invalid, Param Name: { string.Join(", ", context.ModelState.Keys) }, Error Message: { string.Join(", ", exceptionMessages) }");

                var response = new ResponseMessage
                {
                    Header = ResponseMessageExtensions.FailHeader,
                    Detail = string.Join(", ", exceptionMessages),
                    Note = $"Model Invalid, Param Name: {string.Join(", ", context.ModelState.Keys)}"
                };

                context.Result = new BadRequestObjectResult(response);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
