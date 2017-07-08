using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrandChase.Data;
using GrandChase.Net.Client;

namespace GrandChase.Function
{
    public class Guild
    {
        public int GuildID;
        public string MarkName;
        public string GuildName;        

        public void LoadGuilds(int guildid)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT   `MarkName`,  `GuildName` FROM  `gc`.`guilds` WHERE `GuildID` = '{0}'",guildid);

            if (ds.Tables[0].Rows.Count == 0)
            {
                GuildID = 0;
            }
            else
            {
                MarkName = ds.Tables[0].Rows[0]["MarkName"].ToString();
                GuildName = ds.Tables[0].Rows[0]["GuildName"].ToString();
            }
        }

        public void CurrentGuild(ClientSession cs)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT   `GuildID` FROM  `gc`.`guildinfo` WHERE `LoginUID` = '{0}'",cs.LoginUID);

            if (ds.Tables[0].Rows.Count == 0)
            {
                GuildID = 0;
            }
            else
            {
                GuildID = Convert.ToInt32(ds.Tables[0].Rows[0]["GuildID"].ToString());
                LoadGuilds(GuildID);
            }
        }

    }
}
