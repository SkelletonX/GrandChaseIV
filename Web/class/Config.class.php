<?php
Class Config{
	
    private static $Host = "127.0.0.1";
    private static $User = "root";
    private static $Pass = "root";
    private static $Dbsa = "kr_gc3";
    private static $Connect = null;
	
	public $NameGC = 'Grand Chase SkelletonX'; #Nome do Servidor
	
	public $ContasTable = 'accounts'; #Nome da tabelas das contas
	public $LoginsColumn = 'Login'; #Nome da Coluna dos Login
	public $PasswdColumn = 'Passwd'; #Nome da Coluna do Passwd
	public $CheckemailColumn = 'CheckEmail'; #Nome da coluna do checkemail
	public $EmailColumn = 'Email'; #Nome da coluna do email
	public $NickColumn = 'PlayerNick'; #Nome da coluna do Nick do Jogador
	public $KeyEmailColumn = 'EmailKey'; #Nome da coluna da keyemail hash 
	public $GamePointColumn = 'GamePoint'; #Nome da coluna de GamePoint(gp)
	public $isBanColumn = 'isBan'; #Nome de coluna para ver se o player é banido (int)
	public $PlayerOnlineColumn = 'OnlinePlayer'; #Nome da coluna para ver se o player esta online in game (int)
	public $IPColumn = 'IPAddress'; #Nome da coluna para ver o ip do player
	public $AuthLevelColumn = 'AuthLevel'; #Nome da coluna para ver status player mod admin ... dono etc
	
    private static function Conectar(){
        try {
            if(self::$Connect == null):
                $dsn = 'mysql:host=' . self::$Host . ';dbname=' . self::$Dbsa;
                $options = [ PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES UTF8' ];
                self::$Connect = new PDO($dsn, self::$User, self::$Pass, $options);
            endif;
        } catch (PDOException $e) {
            PHPErro($e->getCode(), $e->getMessage(), $e->getFile(), $e->getFile());
        }
        
        self::$Connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        return self::$Connect;
    }
    
    public static function getConn(){
        return self::Conectar();
    }
    
}