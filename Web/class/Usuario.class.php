<?php

class Usuario{
	
	private $login;
	private $email;
	private $gold;
	private $ip;
	private $isBan;
	private $nick;
	private $online;
	private $AuthLevel;
	private $IfEmail;

	function __construct($usuario) {
        $this->login = $usuario;
        $conexao = new Config;
        try{
			$conect = $conexao->getConn();
			$sql = $conect->prepare("SELECT * FROM account WHERE Login = ?");
			$sql->bindParam(1, $usuario);
			$sql->execute();
			$dados = $sql->fetch(PDO::FETCH_ASSOC);
			$this->email = $dados['Email'];
			$this->nick = $dados['Nick'];
			$this->isBan = $dados['isBan'];
			$this->Online = $dados['Online'];
			$this->ip = $dados['Ip'];
			$this->gold = $dados['Gamepoint'];
			$this->AuthLevel = $dados['AuthLevel'];
			$this->IfEmail = $dados['IfEmail'];
			
        }catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
    }
	function getIfEmail(){
		return $this->IfEmail;
	}
	function getAuthLevel(){
		return $this->AuthLevel;
	}
	
	function getOn(){
		return $this->Online;
	}
	
	function isBan() {
        return $this->isBan;
    }
	
	function getNick() {
  		return $this->nick;
	
    }
	
	function getEmail() {
        return $this->email;
    }
	
	function getGP() {
        return number_format($this->gold, 0, ',', ',');
    }
	
	function getLastip() {
        if($this->ip == 0)
		{return "Você nunca logou.";}
	else{
	return $this->ip;}
    }
	
}