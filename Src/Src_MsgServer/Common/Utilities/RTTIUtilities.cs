using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrandChase.Utilities
{
    public static class RTTIUtilities
    {
        public static uint GenerateRTTIHash(string str, uint a1 = 0)
        {
            return ADLER32(Encoding.Unicode.GetBytes(str), a1);
        }

        public static uint ADLER32(byte[] str, uint a1 = 0)
        {
            /*
            uint returnVal = 0;
            uint v8 = 0;
            uint v9 = 0;
            uint v10, v11, v12, v13, v14, v15, v16, v17, v18, v19, v20,
                v21, v22, v23, v24, v25, v26, v27, v28, v29, v30, v31, v32, v33, v34, v35, v36, v37, v38, v39;
            uint v4 = 0;
            uint v5 = 0;
            //if (!string.IsNullOrEmpty(str))
            {
                //byte[] arr = Encoding.Default.GetBytes(str);
                for (uint i = (uint)str.Length; i != 0; a1 %= 0xFFF1u)
                {
                    uint index = 0;
                    v8 = i;
                    if (i >= 5552)
                        v8 = 5552;
                    i -= v8;
                    if (v8 >= 16)
                    {
                        v9 = v8 >> 4;
                        v8 -= 16 * (v8 >> 4);
                        do
                        {
                            v10 = str[index] + a1;
                            v11 = v10 + v5;
                            v12 = str[index + 1] + v10;
                            v13 = v12 + v11;
                            v14 = str[index + 2] + v12;
                            v15 = v14 + v13;
                            v16 = str[index + 3] + v14;
                            v17 = v16 + v15;
                            v18 = str[index + 4] + v16;
                            v19 = v18 + v17;
                            v20 = str[index + 5] + v18;
                            v21 = v20 + v19;
                            v22 = str[index + 6] + v20;
                            v23 = v22 + v21;
                            v24 = str[index + 7] + v22;
                            v25 = v24 + v23;
                            v26 = str[index + 8] + v24;
                            v27 = v26 + v25;
                            v28 = str[index + 9] + v26;
                            v29 = v28 + v27;
                            v30 = str[index + 10] + v28;
                            v31 = v30 + v29;
                            v32 = str[index + 11] + v30;
                            v33 = v32 + v31;
                            v34 = str[index + 12] + v32;
                            v35 = v34 + v33;
                            v36 = str[index + 13] + v34;
                            v37 = v36 + v35;
                            v38 = str[index + 14] + v36;
                            v39 = v38 + v37;
                            v4 = str[index + 15] + v38;
                            v5 = v4 + v39;
                            index += 16;
                            --v9;
                        }
                        while (v9 != 0);
                    }
                    for (; v8 != 0; --v8)
                    {
                        v4 += str[index++];
                        v5 += v4;
                    }
                    v4 %= 0xFFF1u;
                }
                returnVal = v4 | (v5 << 16);
            }
            return returnVal;*/

            uint v3; // ebx@1
            uint v4; // edi@1
            uint v5; // ecx@1
            int v6; // ecx@2
            int v7; // edi@4
            uint result; // eax@6
            int v9 = 0; // esi@7
            uint v10; // ebp@16
            int v11; // eax@17
            int v12; // ecx@18
            int v13; // edi@18
            int v14; // ecx@18
            int v15; // edi@18
            int v16; // ecx@18
            int v17; // edi@18
            int v18; // ecx@18
            int v19; // edi@18
            int v20; // ecx@18
            int v21; // edi@18
            int v22; // ecx@18
            int v23; // edi@18
            int v24; // ecx@18
            int v25; // edi@18
            int v26; // ecx@18
            int v27; // edi@18
            int v28; // ecx@18
            int v29; // edi@18
            int v30; // ecx@18
            int v31; // edi@18
            int v32; // ecx@18
            int v33; // edi@18
            int v34; // ecx@18
            int v35; // edi@18
            int v36; // ecx@18
            int v37; // edi@18
            int v38; // ecx@18
            int v39; // edi@18
            int v40; // ecx@18
            int v41; // edi@18
            uint v42; // eax@22
            int v43; // ecx@23
            int v44; // edi@23
            int v45; // ecx@23
            int v46; // edi@23
            int v47; // ecx@23
            int v48; // edi@23
            int v49; // ecx@23
            int v50; // edi@23
            int v51; // ecx@23
            int v52; // edi@23
            int v53; // ecx@23
            int v54; // edi@23
            int v55; // ecx@23
            int v56; // edi@23
            int v57; // ecx@23
            int v58; // edi@23
            int v59; // ecx@23
            int v60; // edi@23
            int v61; // ecx@23
            int v62; // edi@23
            int v63; // ecx@23
            int v64; // edi@23
            int v65; // ecx@23
            int v66; // edi@23
            int v67; // ecx@23
            int v68; // edi@23
            int v69; // ecx@23
            int v70; // edi@23
            int v71; // ecx@23
            int v72; // edi@23

            v3 = (uint)str.Length;
            v4 = a1 >> 16;
            v5 = (ushort)a1;
            if (str.Length == 1)
            {
                v6 = (int)(str[0] + v5);
                if ((uint)v6 >= 0xFFF1)
                    v6 -= 0xFFF1;
                v7 = (int)(v6 + v4);
                if ((uint)v7 >= 0xFFF1)
                    v7 -= 0xFFF1;
                result = (uint)(v6 | (v7 << 16));
            }
            else
            {
                if (str != null)
                {
                    if (str.Length >= 0x10)
                    {
                        if (str.Length >= 0x15B0)
                        {
                            v10 = (uint)str.Length / 0x15B0;
                            do
                            {
                                v3 -= 5552;
                                v11 = 347;
                                do
                                {
                                    v12 = str[v9] + (int)v5;
                                    v13 = v12 + (int)v4;
                                    v14 = str[v9 + 1] + v12;
                                    v15 = v14 + v13;
                                    v16 = str[v9 + 2] + v14;
                                    v17 = v16 + v15;
                                    v18 = str[v9 + 3] + v16;
                                    v19 = v18 + v17;
                                    v20 = str[v9 + 4] + v18;
                                    v21 = v20 + v19;
                                    v22 = str[v9 + 5] + v20;
                                    v23 = v22 + v21;
                                    v24 = str[v9 + 6] + v22;
                                    v25 = v24 + v23;
                                    v26 = str[v9 + 7] + v24;
                                    v27 = v26 + v25;
                                    v28 = str[v9 + 8] + v26;
                                    v29 = v28 + v27;
                                    v30 = str[v9 + 9] + v28;
                                    v31 = v30 + v29;
                                    v32 = str[v9 + 10] + v30;
                                    v33 = v32 + v31;
                                    v34 = str[v9 + 11] + v32;
                                    v35 = v34 + v33;
                                    v36 = str[v9 + 12] + v34;
                                    v37 = v36 + v35;
                                    v38 = str[v9 + 13] + v36;
                                    v39 = v38 + v37;
                                    v40 = str[v9 + 14] + v38;
                                    v41 = v40 + v39;
                                    v5 = str[v9 + 15] + (uint)v40;
                                    v4 = v5 + (uint)v41;
                                    v9 += 16;
                                    --v11;
                                }
                                while (v11 != 0);
                                v5 %= 0xFFF1u;
                                --v10;
                                v4 %= 0xFFF1u;
                            }
                            while (v10 != 0);
                        }
                        if (v3 != 0)
                        {
                            if (v3 >= 0x10)
                            {
                                v42 = v3 >> 4;
                                do
                                {
                                    v43 = str[v9] + (int)v5;
                                    v44 = v43 + (int)v4;
                                    v45 = str[v9 + 1] + v43;
                                    v46 = v45 + v44;
                                    v47 = str[v9 + 2] + v45;
                                    v48 = v47 + v46;
                                    v49 = str[v9 + 3] + v47;
                                    v50 = v49 + v48;
                                    v51 = str[v9 + 4] + v49;
                                    v52 = v51 + v50;
                                    v53 = str[v9 + 5] + v51;
                                    v54 = v53 + v52;
                                    v55 = str[v9 + 6] + v53;
                                    v56 = v55 + v54;
                                    v57 = str[v9 + 7] + v55;
                                    v58 = v57 + v56;
                                    v59 = str[v9 + 8] + v57;
                                    v60 = v59 + v58;
                                    v61 = str[v9 + 9] + v59;
                                    v62 = v61 + v60;
                                    v63 = str[v9 + 10] + v61;
                                    v64 = v63 + v62;
                                    v65 = str[v9 + 11] + v63;
                                    v66 = v65 + v64;
                                    v67 = str[v9 + 12] + v65;
                                    v68 = v67 + v66;
                                    v69 = str[v9 + 13] + v67;
                                    v70 = v69 + v68;
                                    v71 = str[v9 + 14] + v69;
                                    v72 = v71 + v70;
                                    v5 = str[v9 + 15] + (uint)v71;
                                    v3 -= 16;
                                    v4 = v5 + (uint)v72;
                                    v9 += 16;
                                    --v42;
                                }
                                while (v42 != 0);
                            }
                            for (; v3 != 0; --v3)
                            {
                                v5 += str[v9++];
                                v4 += v5;
                            }
                            v5 %= 0xFFF1u;
                            v4 %= 0xFFF1u;
                        }
                        result = v5 | (v4 << 16);
                    }
                    else
                    {
                        if (str.Length != 0)
                        {
                            do
                            {
                                v5 += str[v9++];
                                v4 += v5;
                                --v3;
                            }
                            while (v3 != 0);
                        }
                        if (v5 >= 0xFFF1)
                            v5 -= 65521;
                        result = v5 | (v4 % 0xFFF1 << 16);
                    }
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }
    }
}
