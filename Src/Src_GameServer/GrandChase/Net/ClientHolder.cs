using Common;
using GrandChase.Data;
using GrandChase.Net.Client;
using System.Collections.Generic;
using System;
using System.Data;

namespace GrandChase.Net
{
    public class ClientHolder : TSingleton<ClientHolder>
    {
        private object _lock = new object();

        private Dictionary<string, ClientSession> _clients { get; set; }

        public ClientHolder()
        {
            _clients = new Dictionary<string, ClientSession>();
        }

        public bool IsLoggedIn( string username )
        {
            lock ( _lock )
            {
                if( _clients.ContainsKey( username.ToLower() ) )
                {
                    return true;
                }
            }

            return false;
        }

        public bool VerifyAccount( string login, string passwd, ClientSession session, ref VerifyAccountResult result )
        {
            /*
            lock ( _lock )
            {
                if( _clients.ContainsKey( login.ToLower() ) )
                {
                    result = VerifyAccountResult.MULTIPLE_CONN_TRY;

                    return true; // Already Logged In
                }

                //                if (!Database.Exists("accounts", "Username = '{0}' AND Password = '{1}'", login, passwd))
                //                {
                //                    result = VerifyAccountResult.WRONG_PASSWD;
                //
                //                    return true;
                //                }

                dynamic datum = new Datum("accounts");

                try
                {
                    datum.Populate( "Username = '{0}'", login, passwd );
                }
                catch( RowNotInTableException )
                {
                    //result = VerifyAccountResult.WRONG_PASSWD;
                    //
                    //return true;

                    Database.ExecuteScript( Database.Host, Database.Username, Database.Password, "INSERT INTO {0}.accounts (Username, Password) VALUES ('{1}', '{2}')", Database.Schema, login, passwd );

                    result = VerifyAccountResult.SUCCESS;

                    try
                    {
                        datum.Populate( "Username = '{0}'", login, passwd );
                    }
                    catch( RowNotInTableException )
                    {
                        result = VerifyAccountResult.INVALID_PROTOCOL;

                        return false;
                    }
                }

                if( datum.Password != passwd )
                {
                    result = VerifyAccountResult.WRONG_PASSWD;

                    return true;
                }

                session.AccountName = login;
                session.Exp0 = datum.Exp0;
                session.Exp1 = datum.Exp1;
                session.Exp2 = datum.Exp2;
                session.GamePoint = datum.GamePoint;
                session.AuthLevel = datum.AuthLevel;

                foreach( dynamic datumItem in new Datums( "inventory" ).Populate( "OwnerLogin = '{0}'", login ) )
                {
                    session.Inventory.Add( new ItemInfo() { GoodsID = datumItem.GoodsID, GoodsUID = datumItem.GoodsUID } );
                }

                foreach( dynamic datumEqp in new Datums( "equipitem" ).Populate( "OwnerLogin = '{0}'", login ) )
                {
                    int Slot = datumEqp.Slot;
                    uint GoodsID = datumEqp.GoodsID;

                    if( Slot >= 0 && Slot < 15 )
                        session.EquipItems[ 0 ].Add( Slot, GoodsID );
                    else if( Slot >= 15 && Slot < 30 )
                        session.EquipItems[ 1 ].Add( Slot - 15, GoodsID );
                    else if( Slot >= 30 && Slot < 45 )
                        session.EquipItems[ 2 ].Add( Slot - 30, GoodsID );
                }

                for( int i = 0; i < session.EquipItems.Length; i++ )
                {
                    for( int j = 0; j < 15; j++ ) // max 15 equips
                    {
                        if( !session.EquipItems[ i ].ContainsKey( j ) )
                        {
                            session.EquipItems[ i ].Add( j, 0xFFFFFFFF );
                        }
                    }
                }

                _clients.Add( login, session );

                result = VerifyAccountResult.SUCCESS;

                return true;
            }
            */
            return true;
        }

        public void DestoryAccount( ClientSession session )
        {
            /*
            if( session.AccountName == null ) return;

            lock ( _lock )
            {
                if( _clients.ContainsKey( session.AccountName.ToLower() ) )
                {
                    _clients.Remove( session.AccountName.ToLower() );
                }
            }
            */
        }
    }
}
