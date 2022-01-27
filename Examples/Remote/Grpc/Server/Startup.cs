﻿using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Remote;
using LinqToDB.Remote.Grpc;
using ProtoBuf.Grpc.Server;

namespace Server
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			// Set up Linq2DB connection
			DataConnection.DefaultSettings = new Linq2DbSettings(
				"Northwind",
				ProviderName.SqlServer,
				"Server=.;Database=Northwind;Trusted_Connection=True"
				);

			services.AddGrpc();
			services.AddCodeFirstGrpc();
			services.AddSingleton(p => (ILinqService)new LinqService() { AllowUpdates = true });
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<GrpcLinqService>();
			});
		}
	}

}
