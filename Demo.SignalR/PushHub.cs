using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Demo.SignalR
{
	public class PushHub : Hub
	{
		/// <summary>
		/// 第一次连接
		/// </summary>
		/// <returns></returns>
		public override Task OnConnected()
		{
			return base.OnConnected();
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		/// <param name="stopCalled"></param>
		/// <returns></returns>
		public override Task OnDisconnected(bool stopCalled)
		{
			string user = ConnectManager.GetUserName(Context.ConnectionId);
			ConnectManager.RemoveUser(user);
			Show(string.Format("{0}退出", user));

			return base.OnDisconnected(stopCalled);
		}

		/// <summary>
		/// 获取当前的用户标识
		/// </summary>
		/// <returns></returns>
		private string GetUserId()
		{
			return Context.QueryString["userId"];
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="content"></param>
		/// <param name="receiveUser"></param>
		public void Show(string content,string receiveUser="")
		{
			string user = ConnectManager.GetUserName(Context.ConnectionId);
			if (string.IsNullOrEmpty(receiveUser))
			{
				Clients.All.show(content);
			}
			else {
				Clients.Client(ConnectManager.GetUserConnect(receiveUser)).show(string.Format("{0}发消息:{1}",user, content));
			}
			
		}

		/// <summary>
		/// 登录操作
		/// </summary>
		/// <param name="user"></param>
		public void Login(string user)
		{

			ConnectManager.OnlineInit(user, Context.ConnectionId);
			Show(string.Format("{0}:登录成功", user));
		}

	}
}