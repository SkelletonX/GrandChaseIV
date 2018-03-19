<?php
class Login {

	public function logar($login, $senha){
		$conexao = new Config;
		 $pw = strtoupper(md5($senha));
		 $id = 1;
		try{
			$conect = $conexao->getConn();
			$strqury = "SELECT * FROM ". $conexao->ContasTable ." WHERE ". $conexao->LoginsColumn ." = ? AND ". $conexao->PasswdColumn ." = ? AND ". $conexao->CheckemailColumn ." = ?";
			$prepare = $conect->prepare($strqury);
            $prepare->bindvalue(1, $login);
			$prepare->bindvalue(2, $pw);
			$prepare->bindvalue(3, $id);
            $prepare->execute();
			$ranking = $prepare->rowCount();
     		if ($ranking >= 1){
                return "<div class='n_ok' style='margin:9px 15px;'><p>Successfully.</p></div>";
            }else{
                return "<div class='n_error' style='margin:9px 15px;'><p>Usuario ou senha incorreta.</p></div>";
            }
		}catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
	}

}