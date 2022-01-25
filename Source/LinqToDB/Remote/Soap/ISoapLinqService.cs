﻿#if NETFRAMEWORK
using System.ServiceModel;

namespace LinqToDB.ServiceModel
{
	[ServiceContract]
	public interface ISoapLinqService
	{
		[OperationContract] LinqServiceInfo GetInfo(string? configuration);
		[OperationContract] int ExecuteNonQuery(string? configuration, string queryData);
		[OperationContract] object? ExecuteScalar(string? configuration, string queryData);
		[OperationContract] string ExecuteReader(string? configuration, string queryData);
		[OperationContract] int ExecuteBatch(string? configuration, string queryData);
	}
}
#endif
