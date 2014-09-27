﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Skpic.Common
{
    public class SimpleDynamic : DynamicObject
    {
        public Dictionary<string, object> Properties = new Dictionary<string, object>();

        public Dictionary<string, object[]> Methods = new Dictionary<string, object[]>();

        public SimpleDynamic()
        {
        }

        public SimpleDynamic(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Properties.Add("P"+i, i.ToString());
            }
        }

        public string GetValue(string key)
        {
            object result;
            return Properties.TryGetValue(key, out result) ? result.ToString() : null;
        }
    }
}
