<?php

	if(isset($_POST['submit'])){
		$msg = $cash->get_cash(@$_SESSION['username']);
        echo "<script>setTimeout(function () {window.location.href= 'index.php';},1500);</script>";		
	}
?>
<div id="main">
	<div class="full_w">
		<div class="h_title">Minha Conta</div>
		<h2>Meus Dados</h2>
		<p>Você pode alterar seus dados aqui.</p>
		
		<div class="entry">
			<div class="sep"></div>
		</div>
		<table style="margin-left: 15px;">
			<tbody>
				<tr>
					<td>Usuario</td>
					<td><?php echo "$_SESSION[username]" ?></td>
					<td>								
					</td>
					<td>								
					</td>
				</tr>
				<tr>
					<td>Senha</td>
					<td>************</td>
					<td>
						<a href="?gc=change_pass" class="table-icon edit" title="Change"></a>
					</td>
					<td>								
					</td>
				</tr>
				<tr>
					<td>E-mail</td>
					<td><?=$user->getEmail();?></td>
					<td>
						<a href="?gc=change_email" class="table-icon edit" title="Change"></a>
					</td>
					<td>								
					</td>
				</tr>
				<tr>
					<td>Tipo de conta</td>
					<td><?php  if($user->getAuthLevel()== 4){ echo "Admin";} elseif($user->getAuthLevel()== 0){ echo "Membro";} elseif ($user->getAuthLevel()== 2){ echo "GM";} ?></td>
					<td>								
					</td>
					<td>								
					</td>
				</tr>
				<tr>
					<td>Status do Email</td>
					<td><?php  if($user->getIfEmail()== 0){echo "Email não foi confirmado";}elseif($user->getIfEmail()==1){echo "Email confirmado";} ?></td>
					<td>								
					</td>
					<td>								
					</td>
				</tr>				
				<tr>
					<td>Ultimo IP</td>
					<td><?=$user->getLastip();?></td>
					<td>
					</td>
					<td>
					</td>
				</tr>
			</tbody>
		</table>
		
		<div class="entry">
			<div class="sep"></div>	 
			<form name="submitForm" action="<?php $PHP_SELF; ?>" method="post"> 
				<a class="button" href="">Atualizar</a>
				<!--<button type="submit" name="submit" class="ok">Ganhar 300 de Cash</button> -->
				<?php if (isset($msg)){echo $msg;}?>
			</form>
		</div>
	</div>
</div>