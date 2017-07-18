<?php

Class Trocar{
	private function teste_pass($pass){
		$conexao = new Config;
		try{
			$conect = $conexao->getConn();
			$prepare = $conect->prepare("SELECT * FROM account WHERE Login = ? AND Passwd = ?");
			$prepare->bindvalue(1, $_SESSION['username']);
			$prepare->bindvalue(2, $pass);
			$prepare->execute();
			$ranking = $prepare->rowCount();
			if ($ranking >= 1){
				return true;
			}else{
				return false;
			}
		}catch(PDOException $e){
			echo "Erro: ".$e->getMessage();
		}
	}	
	
	private function teste_mail($mail){
		$conexao = new Config;
		try{
			$conect = $conexao->getConn();
			$prepare = $conect->prepare("SELECT * FROM account WHERE Login = ? AND Email = ?");
			$prepare->bindvalue(1, $_SESSION['username']);
			$prepare->bindvalue(2, $mail);
			$prepare->execute();
			$ranking = $prepare->rowCount();
			if ($ranking >= 1){
				return true;
			}else{
				return false;
			}
		}catch(PDOException $e){
			echo "Erro: ".$e->getMessage();
		}
	}	
	
	private function change_pass($newpass){
		$conexao = new Config;
		try{
			$conect = $conexao->getConn();
			$prepare = $conect->prepare("UPDATE account SET Passwd = ? WHERE Login = ?");
			$prepare->bindvalue(1, $newpass);
			$prepare->bindvalue(2, $_SESSION['username']);
			$prepare->execute();
			$ranking = $prepare->rowCount();
			if ($ranking >= 1){
				return true;
			}else{
				return false;
			}
		}catch(PDOException $e){
			echo "Erro: ".$e->getMessage();
		}
	}
	
	private function change_mail($newmail){
		$conexao = new Config;
		try{
			$conect = $conexao->getConn();
			$prepare = $conect->prepare("UPDATE account SET email = ? WHERE login = ?");
			$prepare->bindvalue(1, $newmail);
			$prepare->bindvalue(2, $_SESSION['username']);
			$prepare->execute();
			$ranking = $prepare->rowCount();
			if ($ranking >= 1){
				return true;
			}else{
				return false;
			}
		}catch(PDOException $e){
			echo "Erro: ".$e->getMessage();
		}
	}

	public function trocar_senha($oldpass, $newpass, $renewpass){
		
		$pass = strtoupper(md5($oldpass));
		$new_pass = strtoupper(md5($newpass));
		if ($newpass == $renewpass){
			
			if (self::teste_pass($pass)){
				
				if(self::change_pass($new_pass)){
					return "<div class='n_ok'><p>Password changed successfully.</p></div>";
				}else{
					return "<div class='n_error'><p>Something was wrong.</p></div>";
				}
				
			}else{
				return "<div class='n_error'><p>Old Password is wrong.</p></div>";
			}
			
		}else{
			return "<div class='n_error'><p>Passwords do not match.</p></div>";
		}
	}
	
	public function trocar_mail($oldmail, $newmail, $renewmail){
		if ($newmail == $renewmail){
		
			if (self::teste_mail($oldmail)){
				
				if(self::change_mail($newmail)){
					return "<div class='n_ok'><p>E-mail changed successfully.</p></div>";
				}else{
					return "<div class='n_error'><p>Something was wrong.</p></div>";
				}
				
			}else{
				return "<div class='n_error'><p>Old E-Mail is wrong.</p></div>";
			}
			
		}else{
			return "<div class='n_error'><p>E-Mail do not match.</p></div>";
		}
	}
}