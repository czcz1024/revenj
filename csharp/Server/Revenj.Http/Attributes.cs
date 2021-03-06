﻿using System;

namespace Revenj.Http
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ControllerAttribute : Attribute
	{
		public readonly string RootUrl;

		public ControllerAttribute(string rootUrl)
		{
			this.RootUrl = rootUrl;
		}
	}
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class RouteAttribute : Attribute
	{
		public readonly string Path;
		public readonly string Method;

		public RouteAttribute(HTTP http, string path)
		{
			this.Method = http.ToString();
			this.Path = path;
		}
		public RouteAttribute(string path, string method)
		{
			this.Path = path;
			this.Method = method.ToUpperInvariant();
		}
	}
	public enum HTTP
	{
		GET,
		POST,
		PUT,
		DELETE,
		HEAD,
		OPTIONS,
		SEARCH,
		TRACE,
		PATCH
	}
}
