using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Awesomium.Core;

namespace ForgeBot
{
    class FoEResourceInterceptor:ResourceInterceptor
    {
        internal class LoginData
        {
            public string Username;
            public string PasswordHash;
            public string WorldId;
        }
        const string LoginUrl="/start/index?action=ajax_login";
        public string LoginPostUrl
        {
            get { return LoginUrl; }
        }
        public LoginData LastLoadedUserData
        {
            get { return _lastLoginData; }
        }
        private LoginData _lastLoginData;
        protected override ResourceResponse OnRequest(ResourceRequest request)
        {
            // try to recover login data
            if (request.Count>0 && request.Method == "POST" && request.Url.AbsoluteUri.ToLower().EndsWith(LoginUrl))
            {
                _lastLoginData = ParseData(Uri.UnescapeDataString(request[0].Bytes));
            }
            return base.OnRequest(request);
        }


        private static LoginData ParseData(string data)
        {
            string strRegex = @"\{.*\}";
            RegexOptions myRegexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            Regex myRegex = new Regex(strRegex, myRegexOptions);
            
            var loginData=new LoginData();
            foreach (Match myMatch in myRegex.Matches(data))
            {
                if (myMatch.Success)
                {
                    var singles=myMatch.ToString().Substring(1, myMatch.Length - 2).Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                    foreach (var single in singles)
                    {
                        var keyValuePair=single.Replace("\"","").Split(new char[] {':'}, StringSplitOptions.None);
                        switch (keyValuePair[0])
                        {
                            case "name":
                                loginData.Username = keyValuePair[1];
                                break;
                            case "password":
                                loginData.PasswordHash = keyValuePair[1];
                                break;
                            case "world_id":
                                loginData.WorldId = keyValuePair[1];
                                break;
                        }
                    }
                }
            }
            return loginData;
        }
    }
}
