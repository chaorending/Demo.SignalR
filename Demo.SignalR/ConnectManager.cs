using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.SignalR
{
	/// <summary>
	/// 连接管理类
	/// </summary>
	public class ConnectManager
	{
		/// <summary>
		/// 连接记录池
		/// </summary>
		private readonly static ConcurrentDictionary<string, string> _connectPool = new ConcurrentDictionary<string, string>();

		/// <summary>
		/// 添加用户
		/// </summary>
		/// <param name="userKey"></param>
		/// <param name="connection"></param>
		public static void AddUser(string userKey, string connection)
		{
			_connectPool[userKey] = connection;
		}

		/// <summary>
		/// 删除用户
		/// </summary>
		/// <param name="userKey"></param>
		public static void RemoveUser(string userKey)
		{
			string connection = null;
			_connectPool.TryRemove(userKey, out connection);
		}

		/// <summary>
		/// 是否存在连接(是否在线)
		/// </summary>
		/// <param name="receiverId"></param>
		/// <returns></returns>
		public static bool IsOnline(string receiverId)
		{
			return _connectPool.Keys.Contains(receiverId);
		}

		/// <summary>
		/// 推送消息给个人
		/// </summary>
		/// <param name="receiveId"></param>
		/// <param name="msg"></param>
		public static void PushSingleMessage(string receiveId, string msg)
		{
			try
			{
				GetHubContext().Clients.Client(_connectPool[receiveId]).show(msg);
			}
			catch (Exception ex)
			{
				var errMsg = ex.Message;
			}
		}

		/// <summary>
		/// 获取推送上下文
		/// </summary>
		/// <returns></returns>
		public static IHubContext GetHubContext()
		{
			return GlobalHost.ConnectionManager.GetHubContext<PushHub>();
		}

		/// <summary>
		/// 上线初始化
		/// </summary>
		/// <param name="userId">用户ID</param>
		/// <param name="connectionId">连接ID</param>
		public static void OnlineInit(string userId, string connectionId)
		{
			AddUser(userId, connectionId);
		}

		public static string GetUserName(string value)
		{
			return _connectPool.Where(a => a.Value == value).FirstOrDefault().Key;
		}

		public static string GetUserConnect(string userName)
		{
			return _connectPool[userName];
		}
	}
}