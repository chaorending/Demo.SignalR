using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Demo.SignalR.Startup))]

namespace Demo.SignalR
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR(); //声明注册集线器映射
		}
	}
}
