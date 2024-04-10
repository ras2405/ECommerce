using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Exceptions.LogicExceptions;
using System.Security.Authentication;

namespace ECommerceApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message })
                {
                    StatusCode = 404
                };
            }
            else if (context.Exception is BadRequestException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message })
                {
                    StatusCode = 400
                };
            }
            else if (context.Exception is InvalidCredentialException)
            {
                context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message })
                {
                    StatusCode = 401
                };
            }
            else if (context.Exception is Exception e)
            {
                Console.WriteLine($"Message: {e.Message} - StackTrace: {e.StackTrace}");

                context.Result = new ObjectResult(new
                        { Message = "Something went wrong, try again later" })
                        { StatusCode = 500 };
            }
        }
    }
}
