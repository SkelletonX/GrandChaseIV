<?php
error_reporting(0);
echo !extension_loaded('openssl')?"Not Available":"";
require 'PHPMailerAutoload.php';
require 'phpmailer.class.php';
require 'smtp.class.php';

Class Cadastro{
    private $error;
	private $LoginUID;
    
    public function getError(){
        return $this->error;
    }    
    
    public function check_values($login, $senha, $conf_senha, $email){
        if(trim($login) == "" or trim($senha) == "" or trim($conf_senha) == "" or trim($email) == ""):
            $this->error = "Preencha todos os campos.";
            return false;
        else:
            if(self::check_login($login)){
                $this->error = "O usuario ja existe.";
                return false;
            }else{
                if(self::check_email($email)){
                    $this->error = "Este email ja está cadastrado.";
                    return false;
                }else{
                    if($senha !== $conf_senha){
                        $this->error = "As senhas não são compativeis";
                        return false;
                    }else{
                        return true;
                    }
                }
            }
        endif;
    }
    
    private function check_login($login){
        $conexao = new Config;
	try{
            $conect = $conexao->getConn();
            $rank = $conect->prepare("SELECT * FROM account WHERE Login = ?");
            $rank->bindValue(1, $login);
            $rank->execute();
            $ranking = $rank->rowCount();
            if ($ranking >= 1){
                return true;
            }else{
                return false;
            }
	}catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
    }
    
    private function check_email($email){
        $conexao = new Config;
	try{
            $conect = $conexao->getConn();
            $rank = $conect->prepare("SELECT * FROM account WHERE Email = ?");
            $rank->bindValue(1, $email);
            $rank->execute();
            $ranking = $rank->rowCount();
            if ($ranking >= 1){
                return true;
            }else{
                return false;
            }
	}catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
    }
    private function check_nick($nick){
        $conexao = new Config;
	try{
            $conect = $conexao->getConn();
            $rank = $conect->prepare("SELECT * FROM account WHERE Nick = ?");
            $rank->bindValue(1, $nick);
            $rank->execute();
            $ranking = $rank->rowCount();
            if ($ranking >= 1){
                return true;
            }else{
                return false;
            }
	}catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
    }
	public function ifemail($key){
		
		$conexao = new Config;
	try{
            $conect = $conexao->getConn();
            $rank = $conect->prepare("UPDATE account SET IfEmail = ? WHERE KeyEmail = ?");
            $rank->bindValue(1, 1);
			$rank->bindValue(2, $key);
            $rank->execute();
			if($rank->rowCount()){
				return "<div class='n_ok' style='margin:9px 15px;'><p>Email confirmado com sucesso.</p></div>";
			}else{
				
			}
	}catch(PDOException $e){
            echo "Erro: ".$e->getMessage();
        }
		
	}
	public function registro($login, $senha, $confsenha, $email, $nick){
	$pw = strtoupper(md5($senha));
	$pw2 = strtoupper(md5($confsenha));
	if (!self::check_nick($nick)){
		
		if (!self::check_login($login)){
			
			if(!self::check_email($email)){
				
				if($pw == $pw2){
					
					$Rstring = sha1('Key_GCHero'. $email);
					
					$conexao = new Config;
					$conect = $conexao->getConn();
					$rg = $conect->prepare("INSERT INTO account (login, Passwd, Email, Nick, Gamepoint, KeyEmail)VALUES (?, ?, ?, ?, ?, ?);");
					$rg->bindValue(1, $login);
					$rg->bindValue(2, $pw);
					$rg->bindValue(3, $email);
					$rg->bindValue(4, $nick);
					$rg->bindValue(5, 5000); //valor gamepoint
					$rg->bindValue(6, $Rstring);
					$rg->execute();
					//2
					$sql = $conect->prepare("SELECT * FROM account WHERE Login = ?");
					$sql->bindParam(1, $login);
					$sql->execute();
					$dados = $sql->fetch(PDO::FETCH_ASSOC);
					//3
					$rg3 = $conect->prepare("INSERT INTO `character` (LoginUID, CharType, Promotion)VALUES (?, ?, ?);");
					$rg3->bindValue(1, $dados['LoginUID']);
					$rg3->bindValue(2, 1);
					$rg3->bindValue(3, 1);
					$rg3->execute();

					$sts = $rg->rowCount();
					if ($sts >= 1){
						
						$Mailer = new PHPMailer();
						$Mailer->IsSMTP();
						$Mailer->Charset = 'UTF-8';
						$Mailer->SMTPAuth = true;
						$Mailer->SMTPSecure = 'ssl';			
						$Mailer->Host = "mail.grandchasehero.com"; 
						$Mailer->Port = 465;					
						$Mailer->Username = 'confirm@grandchasehero.com';
						$Mailer->Password = '@@@@@@@@@@@@@@@@@@@@@';			
						$Mailer->From = 'confirm@grandchasehero.com';	
						$Mailer->FromName = "GCH";		
						$Mailer->Subject = 'GCHero - Confirme a sua conta.';
						$Body = ('<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office">
						<head>
							<meta charset="UTF-8">
							<meta http-equiv="X-UA-Compatible" content="IE=edge">
							<meta name="viewport" content="width=device-width, initial-scale=1">
							<title>Email</title>
							<link rel="stylesheet" type="text/css" href="../css/email.css">
						<body>
							<center>
								<table align="center" border="0" cellpadding="0" cellspacing="0" height="100%" width="100%" id="bodyTable">
									<tr>
										<td align="center" valign="top" id="bodyCell"
											<table border="0" cellpadding="0" cellspacing="0" width="100%">
												<tr>
													<td align="center" valign="top" id="templateHeader" data-template-container>
														<table align="center" border="0" cellpadding="0" cellspacing="0" width="100%" class="templateContainer">
															<tr>
																<td valign="top" class="headerContainer"><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnImageBlock" style="min-width:100%;">
						<tbody class="mcnImageBlockOuter">
								<tr>
									<td valign="top" style="padding:0px" class="mcnImageBlockInner">
										<table align="left" width="100%" border="0" cellpadding="0" cellspacing="0" class="mcnImageContentContainer" style="min-width:100%;">
											<tbody><tr>
												<td class="mcnImageContent" valign="top" style="padding-right: 0px; padding-left: 0px; padding-top: 0; padding-bottom: 0; text-align:center;">
													
														
															<img align="center" alt="" src="https://gallery.mailchimp.com/0679a0c4b834ffea2bffb2fcf/images/d6e08caa-a896-4550-aef0-c84205dc177c.png" width="315" style="max-width:315px; padding-bottom: 0; display: inline !important; vertical-align: bottom;" class="mcnImage">
														
													
												</td>
											</tr>
										</tbody></table>
									</td>
								</tr>
						</tbody>
					</table></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td align="center" valign="top" id="templateBody" data-template-container>
														<table align="center" border="0" cellpadding="0" cellspacing="0" width="100%" class="templateContainer">
															<tr>
																<td valign="top" class="bodyContainer"><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnTextBlock" style="min-width:100%;">
						<tbody class="mcnTextBlockOuter">
							<tr>
								<td valign="top" class="mcnTextBlockInner" style="padding-top:9px;">
									<table align="left" border="0" cellpadding="0" cellspacing="0" style="max-width:100%; min-width:100%;" width="100%" class="mcnTextContentContainer">
										<tbody><tr>
											
											<td valign="top" class="mcnTextContent" style="padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;">
											
												<center><h1>Confirme sua Conta.</h1></center>

											</td>
										</tr>
									</tbody></table>
								</td>
							</tr>
						</tbody>
					</table><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnDividerBlock" style="min-width:100%;">
						<tbody class="mcnDividerBlockOuter">
							<tr>
								<td class="mcnDividerBlockInner" style="min-width: 100%; padding: 18px 18px 0px;">
									<table class="mcnDividerContent" border="0" cellpadding="0" cellspacing="0" width="100%" style="min-width:100%;">
										<tbody><tr>
											<td>
												<span></span>
											</td>
										</tr>
									</tbody></table>
								</td>
							</tr>
						</tbody>
					</table><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnImageBlock" style="min-width:100%;">
						<tbody class="mcnImageBlockOuter">
								<tr>
									<td valign="top" style="padding:0px" class="mcnImageBlockInner">
										<table align="left" width="100%" border="0" cellpadding="0" cellspacing="0" class="mcnImageContentContainer" style="min-width:100%;">
											<tbody><tr>
												<td class="mcnImageContent" valign="top" style="padding-right: 0px; padding-left: 0px; padding-top: 0; padding-bottom: 0; text-align:center;">
													
														
															<img align="center" alt="" src="https://gallery.mailchimp.com/0679a0c4b834ffea2bffb2fcf/_compresseds/69cb179f-10c7-4cd1-ae3d-b433bd68c061.jpg" width="600" style="max-width:1128px; padding-bottom: 0; display: inline !important; vertical-align: bottom;" class="mcnImage">
														
													
												</td>
											</tr>
										</tbody></table>
									</td>
								</tr>
						</tbody>
					</table><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnTextBlock" style="min-width:100%;">
						<tbody class="mcnTextBlockOuter">
							<tr>
								<td valign="top" class="mcnTextBlockInner" style="padding-top:9px;">
									<table align="left" border="0" cellpadding="0" cellspacing="0" style="max-width:100%; min-width:100%;" width="100%" class="mcnTextContentContainer">
										<tbody><tr>
											
											<td valign="top" class="mcnTextContent" style="padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;">
											
												<p style="text-align: center;">Clique no botão abaixo e confime sua conta no Grand Chase Hero.</p>

											</td>
										</tr>
									</tbody></table>
								</td>
							</tr>
						</tbody>
					</table><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnDividerBlock" style="min-width:100%;">
						<tbody class="mcnDividerBlockOuter">
							<tr>
								<td class="mcnDividerBlockInner" style="min-width: 100%; padding: 9px 18px 0px;">
									<table class="mcnDividerContent" border="0" cellpadding="0" cellspacing="0" width="100%" style="min-width:100%;">
										<tbody><tr>
											<td>
												<span></span>
											</td>
										</tr>
									</tbody></table>
								</td>
							</tr>
						</tbody>
					</table><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnButtonBlock" style="min-width:100%;">
						<tbody class="mcnButtonBlockOuter">
							<tr>
								<td style="padding-top:0; padding-right:18px; padding-bottom:18px; padding-left:18px;" valign="top" align="center" class="mcnButtonBlockInner">
									<table border="0" cellpadding="0" cellspacing="0" class="mcnButtonContentContainer" style="border-collapse: separate !important;border-radius: 3px;background-color: #0091FF;">
										<tbody>
											<tr>
												<td align="center" valign="middle" class="mcnButtonContent" style="font-family: Helvetica; font-size: 18px; padding: 18px;">
													<a class="mcnButton " title="Ativar Conta" href="http://site.grandchasehero.com/cadastrar/key.php?key='.$Rstring.' " target="_self" style="font-weight: bold;letter-spacing: -0.5px;line-height: 100%;text-align: center;text-decoration: none;color: #FFFFFF;">Ativar Conta</a>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table><table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnBoxedTextBlock" style="min-width:100%;">
						<tbody class="mcnBoxedTextBlockOuter">
							<tr>
								<td valign="top" class="mcnBoxedTextBlockInner">
									<table align="left" border="0" cellpadding="0" cellspacing="0" width="100%" style="min-width:100%;" class="mcnBoxedTextContentContainer">
										<tbody><tr>
											
											<td style="padding-top:9px; padding-left:18px; padding-bottom:9px; padding-right:18px;">
											
												<table border="0" cellpadding="18" cellspacing="0" class="mcnTextContentContainer" width="100%" style="min-width:100% !important;">
													<tbody><tr>
														<td valign="top" class="mcnTextContent" style="color: #808080;font-family: Helvetica;font-size: 16px;line-height: 200%;text-align: center;">
															<strong>ATENÇÃO: </strong>Este é um E-mail automático<strong>, não responda.</strong>
														</td>
													</tr>
												</tbody></table>
											</td>
										</tr>
						</body>
					    </html>');
						$Mailer->Body = html_entity_decode($Body);
								
						$Mailer->isHTML(true);
					
						$Mailer->AddAddress($email);
						
						if($Mailer->Send()){
						}else{
							return "<div class='n_error' style='margin:9px 15px;'><p>Falha ao registrar</p></div>";
						}
						
						return "<div class='n_ok' style='margin:9px 15px;'><p>Registrado com sucesso, confirme seu email.</p></div>";
							
					}else{
						return "<div class='n_error' style='margin:9px 15px;'><p>Falha ao registrar.</p></div>";
					}
					
					
				}else{
					
				return "<div class='n_error'><p>Confirmação de senha incorreta.</p></div>";
				}
			}else{
				return "<div class='n_error'><p>Email em uso.</p></div>";
			}
		}else{
			return "<div class='n_error'><p>Login em uso.</p></div>";
		}
	}else{
		return "<div class='n_error'><p>Nick em uso.</p></div>";
	}
}
}