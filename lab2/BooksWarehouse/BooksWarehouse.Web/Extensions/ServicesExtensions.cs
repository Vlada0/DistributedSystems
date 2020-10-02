using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BooksWarehouse.Infrastructure;
using BooksWarehouse.Infrastructure.Models;
using BooksWarehouse.Infrastructure.Validators.Authors;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BooksWarehouse.Web.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)  
        {  
            var handlers = typeof(ICommand).Assembly.GetTypes()  
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface));  
  
            foreach (var handler in handlers)  
            {  
                services.AddTransient(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);  
            }

            return services;
        }

        public static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(opt =>
                {
                    opt.RegisterValidatorsFromAssemblyContaining<AuthorCreateCommandValidator>();
                    opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = ctx =>
                    {
                        var problemDetails = new ValidationProblemDetails(ctx.ModelState)
                        {
                            Title = "One or more validation problems has occured.",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "See the 'errors' property for details.",
                            Instance = ctx.HttpContext.Request.Path
                        };
                        problemDetails.Extensions.Add("traceId", ctx.HttpContext.TraceIdentifier);

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = {"application/problem+json"}
                        };
                    };
                });
            
            return services;
        }
        
        #region oldRegistration

        public static IServiceCollection AddQueryCommandHandlers(this IServiceCollection services)
        {
            var handlerTypes = typeof(ICommand).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(IsHandlerInterface))
                .Where(t => t.Name.EndsWith("Handler"))
                .ToList();
            
            foreach (var type in handlerTypes)
            {
                AddHandler(services, type);
            }

            return services;
        }

        private static void AddHandler(IServiceCollection services, Type type)
        {
            var attributes = type.GetCustomAttributes(false);
            var pipeline = attributes
                .Select(a => a.GetType())
                .Concat(new[] {type})
                .Reverse()
                .ToList();

            var interfaceType = type.GetInterfaces().Single(IsHandlerInterface);
            Func<IServiceProvider, object> factory = BuildPipeline(pipeline, interfaceType);

            services.AddTransient(interfaceType, factory);
        }
        
        private static Func<IServiceProvider, object> BuildPipeline(IEnumerable<Type> pipeline, Type interfaceType)
        {
            var ctors = pipeline
                .Select(x =>
                {
                    var type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                    return type.GetConstructors().Single();
                })
                .ToList();

            object Func(IServiceProvider provider)
            {
                object current = null;

                foreach (var ctor in ctors)
                {
                    var parameterInfos = ctor.GetParameters().ToList();
                    var parameters = GetParameters(parameterInfos, current, provider);
                    current = ctor.Invoke(parameters);
                }

                return current;
            }

            return Func;
        }
        
        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, provider);
            }

            return result;
        }

        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            var parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType))
                return current;

            object service = provider.GetService(parameterType);
            if (service != null)
                return service;

            throw new ArgumentException($"Type {parameterType} not found");
        }
        
        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
        }

        #endregion
    }
}