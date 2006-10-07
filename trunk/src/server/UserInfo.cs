// http://dotnetjunkies.com/WebLog/johnwood/archive/2005/01/05/41688.aspx

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System;

[Serializable]
public class UserInformation : ILogicalThreadAffinative
{
       const string CallContextKey = "UserInfo";
       string userName;

       public UserInformation(string userName)
       {
              this.userName = userName;
       }

       public string UserName
       {
              get
              {
                     return userName;
              }
       }

       public static void SetUser(string userName)
       {
              CallContext.SetData(CallContextKey, new UserInformation(userName));
       }

       public static string GetUser()
       {
              return ((UserInformation) CallContext.GetData(CallContextKey)).UserName;
       }
}
