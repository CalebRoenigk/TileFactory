using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory.Objects
{
    public class Support : Entity
    {
        public Support()
        {
            this.isSolid = true;
            this.hasGravity = false;
        }
    }
}