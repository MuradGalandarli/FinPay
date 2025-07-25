﻿using FinPay.Application.CustomAttributes;
using FinPay.Application.Repositoryes.AppTransactions;
using FinPay.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace FinPay.Presentetion.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;
      
        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name) && name != "stringw@gmail.com")
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var code = $"{(httpAttribute == null ? HttpMethods.Get : httpAttribute.HttpMethods.First())}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";
                
                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name,code);
                if(!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next();

            }
            await next();
        }
    }
}
