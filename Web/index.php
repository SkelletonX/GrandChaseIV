<?php
	session_start();
	require("class/Config.inc.php");	
	date_default_timezone_set('America/Sao_Paulo'); 
	
	if (!isset($_SESSION['username'])):
		echo "<script>window.location = 'login.php';</script>";
		exit;
	endif;
	
	$user = new Usuario($_SESSION['username']);
	$ranking = new Rank;
?>
<html xmlns="http://www.w3.org/1999/xhtml" lang="pl" xml:lang="pl">
	<head>
		<meta http-equiv="content-type" content="text/html; charset=utf-8" />
		<meta name="author" content="SkelletonX" />
		<title>GrandChase Hero</title>
		<link rel="shortcut icon" type="image/png" href="img/icon.ico"/>
		<link rel="stylesheet" type="text/css" href="css/style.css" media="screen" />
		<link rel="stylesheet" type="text/css" href="css/navi.css" media="screen" />
		<script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
	</head>
<body>
<div class="wrap">
	<div id="header"><br><a href="index.php"><img src="img/GrandChase.png" width="250" height="48"></a><br>
		<div id="top">
			<div class="left">
			<br>
				<p>Seja bem vindo : <?php echo "$_SESSION[username]" ?> &nbsp;&nbsp;&nbsp;&nbsp; [ <a href="logout.php">Sair</a> ]
				<?php if($user->getAuthLevel()== 1){ ?>
				<span class="style11"></span><font color="#00FFFF"><?=$ranking->num_contas(); ?></font><span class="style11"> Contas Registradas
				</div>
				<?php }else{ ?>
			</div>
				<?php } ?>
			
			<div class="right">
				<div class="align-right"><br>
					<p><span class="style11">Tipo de conta</span> : <strong style="color:#00FFFF"><?php if($user->getAuthLevel()== 1){ echo "Admin";} elseif($user->getAuthLevel()== 0){ echo "Membro";} elseif ($user->getAuthLevel()== 2){ echo "GM";}?></strong></p>
				</div>
			</div>
	  </div>
	</div>
	
	<div id="content">
		<div id="sidebar">
		  <div class="box">
				<div class="h_title">&#8250; Usuario</div>
				<ul id="home">
					<li class="b2"><a class="icon report" href="?gc=account_info">Dados Cadastrados</a></li>
					<li class="b1"><a class="icon users" href="?gc=player_info">Informações</a></li>
					<?php if($user->getAuthLevel()== 1 ){ ?>
					<li class="b1"><a class="icon category" href="administracao_pbo/">Painel Admin</a></li>
					<?php }else{ 
					}?>
					
					
				</ul>
			</div>
			
			<div class="box">
				<div class="h_title">&#8250; Outros</div>
				<ul>
					<li class="b1"><a class="icon report" href="?gc=download">Baixar Aqui</a></li>
					<li class="b1"><a class="icon add_page" href="?gc=donate">Doação</a></li>
					<li class="b1"><a class="icon page" href="?gc=regras">Regras</a></li>
					<li class="b1"><a class="icon contact" href="suporte/">Suporte</a></li>
				</ul>
			</div>
			<div class="box">
				<div class="h_title">&#8250; Rank</div>
				<ul>
					<li class="b1"><a class="icon category" href="#">Ranking Geral</a></li>
				</ul>
			</div>
		</div>
		<?php
			if(!isset($_GET["gc"]) or $_GET['gc'] == ""){
				$_GET['gc'] = "account_info"; //Pagina padrão
			}
			switch ($_GET['gc']){
				case "account_info":
					include 'pages/myaccount.php';
					break;
				case "player_info":
					include 'pages/player.php';
					break;
				case "change_pass":
					include 'pages/changepassword.php';
					break;
				case "change_email":
					include 'pages/changeemail.php';
					break;
				case "donate":
					include 'pages/donate.php';
					break;
				case "download":
					include 'pages/download.php';
					break;
				default;
					include 'pages/myaccount.php';
					break;
			}
		?>
		
		<div class="clear"></div>
	</div>

	<div id="footer">
		<div class="left">
			<p>GCHero | Copyright 2017 |</p>
		</div>
		<div class="right">
			<p>&copy; 2017</p>
		</div>
	</div>
</div>

</body>
</html>
