﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
   public class Update: Content{
       public string latitude {get;set;}
       public string longitude { get; set; }
       public Update repost {get;set;}
       
    
    }
}
