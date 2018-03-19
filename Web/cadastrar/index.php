<?php
session_start();
require("../class/Config.inc.php");
$register = new Cadastro;
$config = new Config;

if(isset($_POST['submit'])){
	if (trim($_POST['captcha']) != $_SESSION['cap_code']){
		echo "<script>alert('O Captcha esta Errado.');</script><script>window.location='".$redrirac."';</script>";
		exit();
	}else{
		$msg = $register->registro($_POST['txtUsername'], $_POST['txtPassword'], $_POST['txtConPassword'], $_POST['email'], $_POST['Nick']);
		if (strpos($msg, "Successfully") !== false){
			$_SESSION['username'] = $_POST['username'];
			echo "<script>setTimeout(function () {window.location.href= 'index.php';},100);</script>";		
		}
	}
}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="pl" xml:lang="pl">
<head>
<meta http-equiv="content-type" content="text/html; charset=utf-8" />
<meta name="author" content="SkelletonX" />
<link rel="shortcut icon" type="image/png" href="../img/icon.ico"/>
<title>Cadastrar Conta</title>
<link rel="stylesheet" type="text/css" href="../css/login.css" media="screen" />

<script type='text/javascript'>
function check_email(elm){
    var regex_email=/^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*\@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.([a-zA-Z]){2,4})$/
    if(!elm.value.match(regex_email)){
        alert('Please enter a valid email address');
    }else{

}
}
</script>
</head>
<body>
	<div class="wrap">
	<div id="content">
		<p><img src="../img/GrandChase.png" width="400" height="200" /></p>
		<div id="main">
			<div class="full_w">
					<form action="" method="post">
						
					<label for="login">Usuario :</label>
					<input name="txtUsername" type="text" id="txtUsername" class="text" maxlength="10" />

					<label for="pass">Senha :</label>
					<input name="txtPassword" type="password" id="txtPassword" maxlength="10" class="text"/>

					<label for="pass">Conf. Senha :</label>
					<input name="txtConPassword" type="password" id="txtConPassword" maxlength="10" class="text"/>                   

					<label for="email">Email* :</label>
					<input name="email" onblur="check_email(this)" size="45" id="email" type="email" class="text"/>
					
					<label for="Nick">Nick :</label>
					<input name="Nick" type="text" id="Nick" class="text" maxlength="10" />
					
					<img src="../captcha.php" id="captcha" /><br/>
					<input style="margin-top: 5px;" type="text" name="captcha" id="captcha" autocomplete="off" /><br/><br/>
					
					<input type="checkbox" name="check1"><span style="vertical-align: super;">Eu li e aceito os <a href="#">Termos de uso e condiçoes</a></span></input><br/>
					<div class="sep"></div>
					<button type="submit" name="submit" class="ok">Cadastrar</button>
				   <?php if (isset($msg)){echo $msg;}?>
				</form>
		  </div>
			<div style="color:#000;" class="footer"><?php echo $config->NameGC ?> | Copyright 2018</div>
	  </div>
  </div>
</div>
</body>
</html>