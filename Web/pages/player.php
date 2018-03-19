<?php
	$user = new Usuario($_SESSION['username']);
?>
<div id="main">
	<div class="full_w">
		<div class="h_title">Minha Conta</div>
		<h2>Minha Conta</h2>
		<p>Você pode alterar o apelido e ver os caracteres de detalhe aqui!</p>
		
		<div class="entry">
			<div class="sep"></div>
		</div>
		
		<table style="margin-left: 15px;">
			<tbody>
				<tr>
					<td>Apelido</td>
					<td><?=$user->getNick();?></td>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td>GP</td>
					<td><?=$user->getGP();?></td>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td>Banimento</td>
					<td><?php if($user->isBan()== 1){ echo "Você está banido."; }else{ echo "Você nunca foi banido."; }?></td>																		
					<td></td>
					<td></td>
				</tr>
				<tr>						
					<td>Online</td>
					<td><?php if($user->getOn()== 1){ echo "On"; }else{ echo "Off"; } ?></td>
					<td></td>
					<td></td>
				</tr>
			</tbody>
		</table>
		
		<div class='entry'>
			<div class='sep'></div>		
			<a style="margin-left: 15px;margin-bottom:6px" class='button' href=''>Atualizar</a> 
		</div>
		<div class="clear"></div>
	</div>
</div>
