using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
	class ControllerManager
	{
		private Server server;
		private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();

		public ControllerManager(Server server) {
			this.server = server;
			InitController();
		}

		private void InitController()
		{
			DefaultController defaultController = new DefaultController();
			controllerDict.Add(defaultController.RequestCode, defaultController);

			UserController userController = new UserController();
			controllerDict.Add(userController.RequestCode, userController);
		}

		public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client) {
			BaseController controller;
			bool isGet = controllerDict.TryGetValue(requestCode, out controller);
			if (isGet == false) {
				Console.WriteLine("无法得到controller:" + requestCode);
				return;
			}
			string methodName = Enum.GetName(typeof(ActionCode), actionCode);
			MethodInfo  methodInfo =  controller.GetType().GetMethod(methodName);
			if (methodInfo == null) {
				Console.WriteLine("无法得到method:" + actionCode + ":" + methodName);
				return;
			}

			object[] parameters = new object[] { data, client, server };
			object ret = methodInfo.Invoke(controller, parameters);
			if (ret == null || string.IsNullOrEmpty(ret as string)) {
				return;
			}

			server.SendResponse(client, actionCode, ret as string);
		}
	}
}
