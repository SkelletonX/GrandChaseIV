using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class charselect
    {
        public void CharSelectJoin(User user)
        {
            noneInventory InventoryBuff = new noneInventory();
            stregthMaterial stregthmaterial = new stregthMaterial();
            weeklyreward weekly = new weeklyreward();
            monthly Monthly = new monthly();
            matchrank Matchrank = new matchrank();
            herodungeon Hero = new herodungeon();
            changeweapon weapon = new changeweapon();
            novo_character charcardinfo = new novo_character();
            cashlimit virtualcash = new cashlimit();
            baduserinfo baduser = new baduserinfo();
            hellticket Hellticket = new hellticket();
            vipitem Vipitem = new vipitem();
            missionPack missionpack = new missionPack();
            Rainbow rainbow = new Rainbow();
            RKTornado rk = new RKTornado();
            Charismas charismas = new Charismas();
            itemtrade Itemtrade = new itemtrade();
            Npc npc = new Npc();
            itemcharpromotion promotionitem = new itemcharpromotion();
            GpAttribute gpatt = new GpAttribute();
            GpAttributeRandom gpattrandom = new GpAttributeRandom();
            itemattrandom itemrandom = new itemattrandom();
            maxSp maxsplevel = new maxSp();
            GuildLevel guildlevel = new GuildLevel();
            monthlyattend Monthlyattend = new monthlyattend();
            myrankinfo myrank = new myrankinfo();
            userHeroDungeonInfo userHeroInfo = new userHeroDungeonInfo();
            Nasty nasty = new Nasty();
            mymatchrank mymatch = new mymatchrank();
            gachanotice gachanot = new gachanotice();
            loadpoints points = new loadpoints();
            todays todayspopup = new todays();
            SUBSCRIPTION subscription = new SUBSCRIPTION();
            couple Couple = new couple();
            correio Correio = new correio();
            choicebox Choicebox = new choicebox();
            exppotion expPotion = new exppotion();

            InventoryBuff.None_Inventory_Item_List(user);
            stregthmaterial.materialinfo(user);
            weekly.list(user);
            Monthly.list(user);
            Matchrank.rank(user);
            Hero.info(user);
            weapon.changelist(user);
            charcardinfo.info(user);
            virtualcash.virtualcashlimit(user);
            baduser.userInfo(user);
            Hellticket.freemode(user);
            Vipitem.list(user);
            missionpack.list(user);
            rainbow.rainbow(user);
            rk.list(user);
            charismas.charismas(user);
            Itemtrade.list(user);
            npc.gifts(user);
            promotionitem.item(user);
            gpatt.list(user);
            gpattrandom.list(user);
            itemrandom.list(user);
            guildlevel.level(user);
            Monthlyattend.attend(user);
            myrank.info(user);
            userHeroInfo.info(user);
            nasty.getNasty(user);
            mymatch.info(user);
            gachanot.popup(user);
            points.load(user);
            todayspopup.popup(user);
            subscription.sub(user);
            Couple.info(user);
            Correio.Correio(user);
            Choicebox.list(user);
            expPotion.list(user);            
            maxsplevel.maxsplevel(user);
        }
    }
}
