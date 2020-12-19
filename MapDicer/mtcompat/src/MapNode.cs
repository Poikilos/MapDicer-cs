/*
Mtcompat
Copyright(C) 2013 celeron55, Perttu Ahola<celeron55@gmail.com>; 2020 Jake "Poikilos" Gustafson


This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation; either version 2.1 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDicer.mtcompat.src
{
    using content_t = System.UInt16;

    enum LightBank
    {
        LIGHTBANK_DAY,
        LIGHTBANK_NIGHT
    }

    /*
        Simple rotation enum.
    */
    enum Rotation
    {
        ROTATE_0,
        ROTATE_90,
        ROTATE_180,
        ROTATE_270,
        ROTATE_RAND,
    }


    class MapNode : Mapblock
    {

        /*
         Naming scheme:
         - Material = irrlicht's Material class
         - Content = (content_t) content of a node
         - Tile = TileSpec at some side of a node of some content type
        */


        /*
            The maximum node ID that can be registered by mods. This must
            be significantly lower than the maximum content_t value, so that
            there is enough room for dummy node IDs, which are created when
            a MapBlock containing unknown node names is loaded from disk.
        */
        public static uint MAX_REGISTERED_CONTENT = 0x7fffU;

        /*
            A solid walkable node with the texture unknown_node.png.

            For example, used on the client to display unregistered node IDs
            (instead of expanding the vector of node definitions each time
            such a node is received).
        */
        public static uint CONTENT_UNKNOWN = 125;

        /*
            The common material through which the player can walk and which
            is transparent to light
        */
        public static uint CONTENT_AIR = 126;

        /*
            Ignored node.

            Unloaded chunks are considered to consist of this. Several other
            methods return this when an error occurs. Also, during
            map generation this means the node has not been set yet.

            Doesn't create faces with anything and is considered being
            out-of-map in the game map.
        */
        public static byte CONTENT_IGNORE = 127;
        public static byte LIQUID_LEVEL_MASK = 0x07;
        public static byte LIQUID_FLOW_DOWN_MASK = 0x08;

        //public static uint LIQUID_LEVEL_MASK 0x3f // better finite water
        //public static uint LIQUID_FLOW_DOWN_MASK 0x40 // not used when finite water

        /* maximum amount of liquid in a block */
        public static byte LIQUID_LEVEL_MAX = LIQUID_LEVEL_MASK;
        public static byte LIQUID_LEVEL_SOURCE() {
            return (byte)(LIQUID_LEVEL_MAX + 1);
        }
        public static uint LIQUID_INFINITY_MASK = 0x80; //0b10000000

        // mask for leveled nodebox param2
        public static byte LEVELED_MASK = 0x7F;
        public static byte LEVELED_MAX = LEVELED_MASK;

        public virtual byte GetLevel(INodeDefManager nodemgr)
        {
            return 255;
        }
    }

    class LiquidMapNode : MapNode
    {
        byte param2;
        byte getParam2()
        {
            return param2;
        }
        public override byte GetLevel(INodeDefManager nodemgr)
        {
            ContentFeatures f = nodemgr.Get(this);
            if (f.liquid_type == LiquidType.LIQUID_SOURCE)
                return LIQUID_LEVEL_SOURCE();
            if (f.param_type_2 == ContentParamType2.CPT2_FLOWINGLIQUID)
                return (byte)(getParam2() & LIQUID_LEVEL_MASK);
            if (f.liquid_type == LiquidType.LIQUID_FLOWING) // can remove if all param_type_2 setted
                return (byte)(getParam2() & LIQUID_LEVEL_MASK);
            if (f.param_type_2 == ContentParamType2.CPT2_LEVELED)
            {
                byte level = (byte)(getParam2() & LEVELED_MASK);
                if (level != 0)
                    return level;
            }
            if (f.leveled > MapNode.LEVELED_MAX)
                return MapNode.LEVELED_MAX;
            return f.leveled;

        }
    }
}
