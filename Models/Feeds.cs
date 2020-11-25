using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterApiMVC.Model
{
    public class Feeds
    {
        public long id { get; set; }
        public string tittle { get; set; }
        public string text { get; set; }
        public string hastag { get; set; }
        public DateTime tweetDate { get; set; }
    }
}
