﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekarJYPS
{
    public abstract class Player
    {
        public PieceColor Color { get; protected set; }
    }

    public enum Players
    {
        Human, AI
    }
}
