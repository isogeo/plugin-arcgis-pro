﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcMapAddinIsogeo.Objects
{
    public class comboItem
    {
        public string code { get; set; }
        public string value { get; set; }

        public comboItem(String code,String value)
        {
            this.code = code;
            this.value = value;
        }

        
        public override string ToString()
        {
            return value;
        }
    }
}
