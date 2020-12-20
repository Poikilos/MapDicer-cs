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

namespace MapDicer.MtCompat.src
{
    abstract class Database
    {
        public abstract void beginSave();
        public abstract void endSave();
        public abstract bool initialized();

        /// <summary>
        /// Convert unsigned to signed.
        /// See
        /// https://git.minetest.org/minetest/minetest/src/branch/master/src/database.cpp
        /// </summary>
        public static short unsigned_to_signed(ushort i, ushort max_positive)
        {
            // originally 16-bit params and return
            if (i < max_positive)
            {
                return (short)i;
            }
            return (short)(i - (max_positive * 2));
        }

        /// <summary>
        /// "Modulo of a negative number does not work consistently in C"
        /// See
        /// https://git.minetest.org/minetest/minetest/src/branch/master/src/database.cpp
        /// </summary>
        public static long pythonmodulo(long i, short mod)
        {
            // ^ mod was originally 16-bit
            if (i >= 0)
            {
                return i % mod;
            }
            return mod - ((-i) % mod);
        }
    }

    class MapDatabase : Database
    {
        /// <summary>
        /// Convert a mapblock offset (1 per mapblock) to a unique Mapblock Id given 3D
        /// Coordinates (this part is like Minetest and shouldn't be called directly except
        /// for 3D voxel applications). For compatibility with Minetest position hashes, the unusual
        /// type casting must remain.
        /// See
        /// https://git.minetest.org/minetest/minetest/src/branch/master/src/database.cpp
        /// </summary>
        /// <param name="p">A block offset in [x, z] format where y is zenith and 1 is the
        /// size of a Mapblock in this Lod</param>
        /// <returns>a primary key for the Mapblock at the given location</returns>
        public long getBlockAsInteger(v3s16 pos)
        {
            return (long)((ulong)pos.Z * 0x1000000 +
                (ulong)pos.Y * 0x1000 +
                (ulong)pos.X);
        }
        /// <summary>
        /// This is a hashing algorithm so for compatibility with Minetest it must stay intact
        /// even though it does some things inside out.
        /// See
        /// https://git.minetest.org/minetest/minetest/src/branch/master/src/database.cpp#L59
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public v3s16 getIntegerAsBlock(long i)
        {
            v3s16 pos;
            // barring the "new" operator, fields must be assigned manually before use in C# structs.
            pos.X = unsigned_to_signed((ushort)pythonmodulo(i, 4096), 2048);
            i = (i - pos.X) / 4096;
            pos.Y = unsigned_to_signed((ushort)pythonmodulo(i, 4096), 2048);
            i = (i - pos.Y) / 4096;
            pos.Z = unsigned_to_signed((ushort)pythonmodulo(i, 4096), 2048);
            return pos;
        }

        public override void beginSave()
        {
            throw new NotImplementedException();
        }

        public override void endSave()
        {
            throw new NotImplementedException();
        }

        public override bool initialized()
        {
            throw new NotImplementedException();
        }
    }
}
