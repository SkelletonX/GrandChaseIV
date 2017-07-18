<?php
Class Config{
    private static $Host = "192.95.4.5";
    private static $User = "root";
    private static $Pass = "my@_Sk321";
    private static $Dbsa = "gc";
    private static $Connect = null;
    
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