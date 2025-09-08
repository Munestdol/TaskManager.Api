﻿using FluentValidation;
using System.Net;
using System.Text.Json;

namespace TaskManager.Api.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { Errors = errors }));
            }
        }
    }
}
