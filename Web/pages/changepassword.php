<?php
	$trocar = new Trocar;

	if(isset($_POST['go'])){
		$msg = $trocar->trocar_senha($_POST['oldpass'], $_POST['newpassword'], $_POST['rpassword']);
	}
?>
<div id="main">
	<div class="full_w">
		<div class="h_title">Meus Dados</div>
		<h2>Trocar Senha</h2>
		<p>Aqui você pode alterar sua senha.</p>
		
		<div class="entry">
			<div class="sep"></div>
		</div>
		
		<?php if (isset($msg)){echo $msg;}?>					
		<form name="submit" action="<?php $PHP_SELF; ?>" method="post">	
			<table>
				<tbody>
					<tr>
						<td>Senha Atual</td>
						<td><input name="oldpass" type="password" maxlength="15" class="text" style="text-align:center"></td>
						<td>								
						</td>
						<td>								
						</td>
					</tr>
					<tr>
						<td>Nova Senha</td>
						<td><input name="newpassword" type="password" maxlength="15" class="text" style="text-align:center"></td>
						<td>								
						</td>
						<td>								
						</td>
					</tr>
					<tr>
						<td>Confirmar Senha</td>
						<td><input name="rpassword" type="password" maxlength="15" class="text" style="text-align:center"></td>
						<td>								
						</td>
						<td>								
						</td>
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