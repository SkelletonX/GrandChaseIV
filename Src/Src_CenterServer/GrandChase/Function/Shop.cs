using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrandChase.IO.Packet;
using GrandChase.Net.Client;
using GrandChase.Data;
using GrandChase.Security;
using GrandChase.IO;
using Manager.Factories;
using Manager;

namespace GrandChase.Function
{
    public class Shop
    {
        public int CountBuy = 0;
        public struct goodsItems
        {
            public int goodsid;
            public string goodsName;
            public byte itemType;
            public int Price;
            public int chartype;
        }
        public goodsItems[] goodsIttems = new goodsItems[0];

        public void SendShop()
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM `gc`.`goodsinfolist`");

            // 캐릭터 배열 사이즈 늘리기
            Array.Resize(ref goodsIttems, ds.Tables[0].Rows.Count);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                goodsIttems[i].goodsid = Convert.ToInt32(ds.Tables[0].Rows[i]["GoodsID"].ToString());
                goodsIttems[i].goodsName = Convert.ToString(ds.Tables[0].Rows[i]["GoodsName"].ToString());
                goodsIttems[i].itemType = Convert.ToByte(ds.Tables[0].Rows[i]["itemtype"].ToString());
                goodsIttems[i].Price = Convert.ToInt32(ds.Tables[0].Rows[i]["price"].ToString());
                goodsIttems[i].chartype = Convert.ToInt32(ds.Tables[0].Rows[i]["chartype"].ToString());
            }
        }
    }
}
