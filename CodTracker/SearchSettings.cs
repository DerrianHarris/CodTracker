using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodTracker.Model
{
    public class SearchSettings
    {
        public string Username;
        public string Platform;
        public string Mode;
        public JObject Json;
    }
}
