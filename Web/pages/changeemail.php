<?php
	$trocar = new Trocar;

	if(isset($_POST['go'])){
		$msg = $trocar->trocar_mail($_POST['oldpassword'], $_POST['newpassword'], $_POST['rpassword']);
	}
?>
<div id="main">
	<div class="full_w">
		<div class="h_title">Informações de usuario</div>
		<h2>Trocar Email</h2>
		<p>Você pode alterar sua senha e detalhes de e-mail aqui.</p>
		
		<div class="entry">
			<div class="sep"></div>
		</div>
		
		<?php if (isset($msg)){echo $msg;}?>						
		<form name="submit" action="<?php $PHP_SELF; ?>" method="post">	
			<table>
				<tbody>
					<tr>							
						<td>Email Atual</td>
						<td><input name="oldpassword" type="text" value="<?=$user->getEmail();?>" maxlength="40" class="text" style="text-align:center" readonly></td>							
						<td></td>
						<td></td>
					</tr>
					<tr>
						<td>Novo Email</td>
						<td><input name="newpassword" type="text" maxlength="40" class="text" style="text-align:center"></td>
						<td></td>
						<td></td>
					</tr>	
					<tr>
						<td>Confirme o Email</td>
						<td><input name="rpassword" type="text" maxlength="40" class="text" style="text-align:center"></td>
						<td></td>
						<td></td>
					</tr>
				</tbody>
			</table>
			
			<div class="entry" style="margin-bottom: 9px;">
				<div class="sep"></div>	 
				<a class="button" href="index.php">Voltar</a> 
				<input type="submit" name="go" class="button" value="Salvar"></input> 
			</div>	
		</form>
	</div>
</div>