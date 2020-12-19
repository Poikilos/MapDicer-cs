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
    enum ContentParamType
    {
        CPT_NONE,
        CPT_LIGHT,
    };
    enum ContentParamType2
    {
        CPT2_NONE,
        // Need 8-bit param2
        CPT2_FULL,
        // Flowing liquid properties
        CPT2_FLOWINGLIQUID,
        // Direction for chests and furnaces and such
        CPT2_FACEDIR,
        // Direction for signs, torches and such
        CPT2_WALLMOUNTED,
        // Block level like FLOWINGLIQUID
        CPT2_LEVELED,
        // 2D rotation for things like plants
        CPT2_DEGROTATE,
        // Mesh options for plants
        CPT2_MESHOPTIONS,
        // Index for palette
        CPT2_COLOR,
        // 3 bits of palette index, then facedir
        CPT2_COLORED_FACEDIR,
        // 5 bits of palette index, then wallmounted
        CPT2_COLORED_WALLMOUNTED,
        // Glasslike framed drawtype internal liquid level, param2 values 0 to 63
        CPT2_GLASSLIKE_LIQUID_LEVEL,
    }
    enum LiquidType
    {
        LIQUID_NONE,
        LIQUID_FLOWING,
        LIQUID_SOURCE,
    }
    public class NodeDef
    {
        
    }
    interface INodeDefManager
    {
        ContentFeatures Get(MapNode node);
        // TODO: C# 8.0 or greater allows an interface to have a default implementation.
    }
}
