<?php


class Rank {
	
	public function num_contas(){
		$conexao = new Config;
		try{
			$conect = $conexao->getConn();
			$prepare = $conect->prepare("SELECT * FROM account");
            $prepare->execute();
			$ranking = $prepare->rowCount();
			return $ranking;
		}catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
	}
}