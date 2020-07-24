using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AccountManagement.Api.Controllers;
using AccountManagement.Api.WebMiddleware;
using AccountManagement.Business.AccountDomain;
using AccountManagement.ConfigSection;
using AccountManagement.ConfigSection.ConfigModels;
using AccountManagement.Consumers;
using AccountManagement.Data;
using AccountManagement.Utility.IntegrationEventPublisherSection.Imp;
using DotNetCore.CAP;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AccountManagement
{
    public class Startup
    {
        private const string ALLOWED_ORIGIN_POLICY = "AllowedOriginPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                             {
                                 options.AddPolicy(ALLOWED_ORIGIN_POLICY,
                                                   builder =>
                                                   {
                                                       builder.AllowAnyHeader()
                                                              .AllowAnyMethod()
                                                              .AllowAnyOrigin();
                                                   });
                             });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                                       {
                                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                           options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                                           options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.Default;
                                           options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
                                           options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                           options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                                           options.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                                       })
                    .AddApplicationPart(typeof(HomeController).Assembly);

            #region Swagger

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo()); });

            #endregion

            #region Db

            DbOption dbOption = AppConfigs.SelectedDbOption();

            switch (dbOption.DbType)
            {
                case DbTypes.SqlServer:
                    services.AddDbContext<DataContext>(builder => builder.UseSqlServer(dbOption.ConnectionStr));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion

            #region RabbitMQ

            RabbitMQConfigModel rabbitMqConfigModel = AppConfigs.GetRabbitMQConfigModel();
            RabbitMQOption rabbitMqOption = rabbitMqConfigModel.SelectedRabbitMQOption();

            services.AddSingleton(rabbitMqConfigModel);

            Type interfaceType = typeof(ICapSubscribe);
            IEnumerable<Type> types = typeof(AccountCreated_VerificationMailSender)
                                     .Assembly
                                     .GetTypes()
                                     .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            foreach (Type t in types)
            {
                services.Add(new ServiceDescriptor(typeof(ICapSubscribe), t, ServiceLifetime.Transient));
            }

            services.AddCap(configurator =>
                            {
                                configurator.UseEntityFramework<DataContext>();
                                configurator.UseRabbitMQ(options =>
                                                         {
                                                             options.UserName = rabbitMqOption.UserName;
                                                             options.Password = rabbitMqOption.Password;
                                                             options.HostName = rabbitMqOption.HostName;
                                                             options.VirtualHost = rabbitMqOption.VirtualHost;
                                                         }
                                                        );
                            }
                           );

            #endregion

            #region IntegrationEventPublisher

            services.AddSingleton<ICapIntegrationEventPublisher, CapIntegrationEventPublisher>();

            #endregion

            #region HealthCheck

            IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

            healthChecksBuilder.AddUrlGroup(new Uri($"{AppConfigs.AppUrls().First()}/health-check"), HttpMethod.Get, name: "HealthCheck Endpoint");

            switch (dbOption.DbType)
            {
                case DbTypes.SqlServer:
                    healthChecksBuilder.AddSqlServer(dbOption.ConnectionStr, name: "Sql Server");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (rabbitMqOption.BrokerType)
            {
                case MessageBrokerTypes.RabbitMq:
                    string rabbitConnStr = $"amqp://{rabbitMqOption.UserName}:{rabbitMqOption.Password}@{rabbitMqOption.HostName}:5672{rabbitMqOption.VirtualHost}";
                    healthChecksBuilder.AddRabbitMQ(rabbitConnStr, sslOption: null, name: "RabbitMq", HealthStatus.Unhealthy, new[] {"rabbitmq"});
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            services
               .AddHealthChecksUI(setup =>
                                  {
                                      setup.MaximumHistoryEntriesPerEndpoint(50);
                                      setup.AddHealthCheckEndpoint("StockManagement Project", $"{AppConfigs.AppUrls().First()}/healthz");
                                  })
               .AddInMemoryStorage();

            #endregion

            services.AddScoped<IAccountService, AccountService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GeneralExceptionHandlerMiddleware>();
            app.Use((async (httpContext, next) =>
                     {
                         if (httpContext.Request.Headers.TryGetValue("x-trace-id", out StringValues stringValues))
                         {
                             httpContext.TraceIdentifier = stringValues;
                         }

                         httpContext.TraceIdentifier ??= Guid.NewGuid().ToString();
                         await next();
                     }));

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", ""); });
            app.UseHealthChecksUI();
            app.UseHealthChecks("/healthz", new HealthCheckOptions
                                            {
                                                Predicate = _ => true,
                                                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                                            });


            app.UseRouting();
            app.UseCors(ALLOWED_ORIGIN_POLICY);
            app.UseEndpoints(builder => { builder.MapControllers(); });
        }
    }
}