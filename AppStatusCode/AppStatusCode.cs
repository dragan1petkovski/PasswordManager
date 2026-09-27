using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace ApplicationStatusCode
{
	public class AppStatusCode<T>
	{
		[JsonInclude]
		private StatusEnum status;
		[JsonInclude]
		private string message;

		private int statusCode;

		private AppStatusCode(int statusCode, StatusEnum httpstatuscode, string response)
		{
			this.statusCode = statusCode;
			this.status = httpstatuscode;
			this.message = response;
		}
		
		public int ToHTTPCode()
		{
			return this.statusCode;
		}

		public static AppStatusCode<T> ItemExist = new AppStatusCode<T>(409,StatusEnum.Failed, $"{typeof(T).Name} Already Exist");
		public static AppStatusCode<T> ItemDontExist = new AppStatusCode<T>(404, StatusEnum.Failed, $"{typeof(T).Name} Don't Exist");
		public static AppStatusCode<T> AddNewItem = new AppStatusCode<T>(201,StatusEnum.Success, $"New {typeof(T).Name} Added");
		public static AppStatusCode<T> DeleteItem = new AppStatusCode<T>(200, StatusEnum.Success, $"{typeof(T).Name} Deleted");
		public static AppStatusCode<T> UpdateItem = new AppStatusCode<T>(200, StatusEnum.Success, $"{typeof(T).Name} Updated");
		public static AppStatusCode<T> ServiceUnavailable = new AppStatusCode<T>(503, StatusEnum.Failed, "Service Unavailable");
		public static AppStatusCode<T> UnauthorizedAccess = new AppStatusCode<T>(401, StatusEnum.Failed, "Unauthorized Access");
		public static AppStatusCode<T> AccessDenied = new AppStatusCode<T>(403, StatusEnum.Failed, "Access Denied");
		public static AppStatusCode<T> ADSyncFailed = new AppStatusCode<T>(504, StatusEnum.Failed, "Sychronization Failed");
		public static AppStatusCode<T> ADSyncSuccess = new AppStatusCode<T>(200, StatusEnum.Success, "AD Sync Successful");
		public static AppStatusCode<T> InvalidRequest = new AppStatusCode<T>(409, StatusEnum.Failed, "Invalid Request");
		public static AppStatusCode<T> SuccessfulMembershipUpdate = new AppStatusCode<T>(200, StatusEnum.Success, $"Succesfull {typeof(T).Name} membership update");
		public static AppStatusCode<T> FailedMembershipUpdate = new AppStatusCode<T> (503, StatusEnum.Failed, $"Faild {typeof(T).Name} membership update");

	}
}
