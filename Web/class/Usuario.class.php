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
			$sql = $conect->prepare("SELECT * FROM ". $conexao->ContasTable ." WHERE ". $conexao->LoginsColumn ." = ?");
			$sql->bindParam(1, $usuario);
			$sql->execute();
			$dados = $sql->fetch(PDO::FETCH_ASSOC);
			$this->email = $dados[$conexao->EmailColumn];
			$this->nick = $dados[$conexao->NickColumn];
			$this->isBan = $dados[$conexao->isBanColumn];
			$this->Online = $dados[$conexao->PlayerOnlineColumn];
			$this->ip = $dados[$conexao->IPColumn];
			$this->gold = $dados[$conexao->GamePointColumn];
			$this->AuthLevel = $dados[$conexao->AuthLevelColumn];
			$this->IfEmail = $dados[$conexao->CheckemailColumn];
			
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