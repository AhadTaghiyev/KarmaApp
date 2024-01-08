using System;
using System.Net;
using Karma.Core.DTOS;
using Karma.Service.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Karma.Service.CustomMiddlewares
{
	public static class ExceptionMiddlewareExtensions
	{
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    int statusCode = 500;
                    string message = "Server internal error";

                    //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>().Error;

                    if(contextFeature  is ItemNotFoundExcpetion)
                    {
                        statusCode = 404;
                        message = contextFeature.Message;
                    }
                    context.Response.ContentType = "application/json";
                    var obj = new
                    {
                        status = statusCode,
                        message = contextFeature.Message
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(obj));
                });
            });
        }
    }
}

