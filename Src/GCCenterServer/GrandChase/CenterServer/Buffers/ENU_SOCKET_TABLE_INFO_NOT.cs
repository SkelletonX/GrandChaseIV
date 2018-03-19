﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    class SocketTableinf
    {
        public void SocketTable(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(15);
            Write.Header();
            Write.Hex("00 00 00 65 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 02 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 03 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 04 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 05 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 08 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 09 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 0A 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 0B 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 0C 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 0D 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 0E 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 0F 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 10 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 11 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 12 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 13 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 14 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 15 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 16 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 17 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 18 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 19 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 1A 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 1B 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 1C 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 1D 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 1E 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 1F 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 20 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 21 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 22 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 23 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 24 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 25 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 26 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 27 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 28 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 29 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 2A 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 2B 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 2C 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 2D 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 2E 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 2F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 30 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 31 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 32 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 33 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 34 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 35 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 36 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 37 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 38 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 39 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 3A 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 3B 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 3C 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 3D 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 3E 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 3F 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 40 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 41 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 42 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 43 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 44 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 45 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 46 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 47 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 48 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 49 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 4A 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 4B 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 4C 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 4D 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 4E 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 4F 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 50 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 51 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 52 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 53 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 54 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 55 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 56 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 57 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 58 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 59 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 5A 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 5B 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 5C 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 5D 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 5E 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 5F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 60 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 61 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 62 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 63 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 25 00 00 00 64 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 25 00 00 00 65 00 00 00 00 00 00 00 BE 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 00 01 00 00 00 BE 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 00 02 00 00 00 BE 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 00 03 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 00 04 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 00 05 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 00 06 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 00 07 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 00 08 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 00 09 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 00 0A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 00 0B 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 00 0C 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 00 0D 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 00 0E 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 00 0F 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 00 10 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 00 11 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 00 12 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 00 13 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 00 14 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 00 15 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 00 16 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 00 17 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 00 18 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 00 19 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 00 1A 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 00 1B 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 00 1C 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 00 1D 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 00 1E 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 00 1F 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 00 20 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 00 21 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 00 22 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 00 23 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 00 24 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 00 25 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 00 26 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 00 27 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 00 00 28 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 00 00 29 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 00 00 2A 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 00 00 2B 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 00 00 2C 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 00 00 2D 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 01 47 58 00 00 00 2E 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 01 47 58 00 00 00 2F 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 01 47 58 00 00 00 30 00 00 F8 70 00 01 1D 28 00 01 47 58 00 01 7D 40 00 00 00 31 00 00 F8 70 00 01 1D 28 00 01 47 58 00 01 7D 40 00 00 00 32 00 00 F8 70 00 01 1D 28 00 01 47 58 00 01 7D 40 00 00 00 33 00 01 1D 28 00 01 47 58 00 01 7D 40 00 01 B7 74 00 00 00 34 00 01 1D 28 00 01 47 58 00 01 7D 40 00 01 B7 74 00 00 00 35 00 01 1D 28 00 01 47 58 00 01 7D 40 00 01 B7 74 00 00 00 36 00 01 47 58 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 00 00 37 00 01 47 58 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 00 00 38 00 01 47 58 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 00 00 39 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 00 00 3A 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 00 00 3B 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 00 00 3C 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 00 00 3D 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 00 00 3E 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 00 00 3F 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 03 15 10 00 00 00 40 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 03 15 10 00 00 00 41 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 03 15 10 00 00 00 42 00 02 50 F8 00 02 AC C4 00 03 15 10 00 03 BB 78 00 00 00 43 00 02 50 F8 00 02 AC C4 00 03 15 10 00 03 BB 78 00 00 00 44 00 02 50 F8 00 02 AC C4 00 03 15 10 00 03 BB 78 00 00 00 45 00 02 AC C4 00 03 15 10 00 03 BB 78 00 04 27 48 00 00 00 46 00 02 AC C4 00 03 15 10 00 03 BB 78 00 04 27 48 00 00 00 47 00 02 AC C4 00 03 15 10 00 03 BB 78 00 04 27 48 00 00 00 48 00 03 15 10 00 03 BB 78 00 04 27 48 00 04 AC E0 00 00 00 49 00 03 15 10 00 03 BB 78 00 04 27 48 00 04 AC E0 00 00 00 4A 00 03 15 10 00 03 BB 78 00 04 27 48 00 04 AC E0 00 00 00 4B 00 03 BB 78 00 04 27 48 00 04 AC E0 00 05 32 78 00 00 00 4C 00 03 BB 78 00 04 27 48 00 04 AC E0 00 05 32 78 00 00 00 4D 00 03 BB 78 00 04 27 48 00 04 AC E0 00 05 32 78 00 00 00 4E 00 04 27 48 00 04 AC E0 00 05 32 78 00 05 B8 10 00 00 00 4F 00 04 27 48 00 04 AC E0 00 05 32 78 00 05 B8 10 00 00 00 50 00 04 27 48 00 04 AC E0 00 05 32 78 00 05 B8 10 00 00 00 51 00 04 AC E0 00 05 32 78 00 05 B8 10 00 06 3D A8 00 00 00 52 00 04 AC E0 00 05 32 78 00 05 B8 10 00 06 3D A8 00 00 00 53 00 04 AC E0 00 05 32 78 00 05 B8 10 00 06 3D A8 00 00 00 54 00 05 32 78 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 00 00 55 00 05 32 78 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 00 00 56 00 05 32 78 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 00 00 57 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 07 48 74 00 00 00 58 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 07 48 74 00 00 00 59 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 07 48 74 00 00 00 5A 00 06 3D A8 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 00 00 5B 00 06 3D A8 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 00 00 5C 00 06 3D A8 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 00 00 5D 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 00 00 5E 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 00 00 5F 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 00 00 60 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 00 00 61 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 00 00 62 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 00 00 63 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 09 5E D4 00 00 00 64 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 09 5E D4 00 04 61 54 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
