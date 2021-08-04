using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PJMAN1_DEMO_.Models
{
    public class ViewModel
    {
        public List<Tasks> Tasks { get; set; }

        public List<BTasks> BTasks { get; set; }

        public List<CTasks> CTasks { get; set; }
    }
}