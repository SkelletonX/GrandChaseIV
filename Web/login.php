<?php
	session_start();
    require("class/Config.inc.php");
	$login = new Login;
	
	if(isset($_SESSION["username"])):
        echo "<script>window.location.href= 'index.php';</script>";
    endif;
	
	if(isset($_POST['submit'])){
		$msg = $login->logar($_POST['username'], $_POST['password']);
		if (strpos($msg, "Successfully") !== false){
			$_SESSION['username'] = $_POST['username'];
			echo "<script>setTimeout(function () {window.location.href= 'index.php';},100);</script>";		
		}
	}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="pl" xml:lang="pl">
	<head>
		<meta http-equiv="content-type" content="text/html; charset=utf-8" />
		<meta name="author" content="SkelletonX" />
		<title>Grand Chase Hero</title>
		<link rel="shortcut icon" type="image/png" href="img/icon.ico"/>
		<link rel="stylesheet" type="text/css" href="css/login.css" media="screen" />
	</head>
	
	<body>
		<div class="wrap">
			<div id="content">
				<p><img src="http://site.grandchasehero.com/img/GrandChase.png" width="400" height="200" /></p>	
				<div id="main">
					<div class="full_w">
						<form action="" method="post">
							<center><h2>Login</h2></center>
							<div class="sep"></div>
							<label for="login">Nome de Usuario:</label>
							<input id="username" name="username" class="text"/>
							<label for="pass">Senha:</label>
							<input id="password" name="password" type="password" class="text"/>
							<?php if (isset($msg)){echo $msg;}?>
							<div class="sep"></div>
							<button type="submit" name="submit" class="ok">Entrar</button> 
							<button style="float: right;"type="button" class="reg" onclick="window.location.href='cadastrar/'">Registrar</button>
						</form>
					</div>
					
					<div class="footer"><li><font color="#000000">GrandChase Hero | Copyright 2017</font></div>
				</div>
			</div>
		</div>
	</body>
</html>
