<?php
session_start();
error_reporting(0);
require("../class/Config.inc.php");
if(!isset($_GET['key'])){
	session_destroy();
	Header("Location: ..");
}else{
$key = $_GET['key'];
$register = new Cadastro;
$register->ifemail($key);
}
?>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office">
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
											
												<h1>Parabéns conta verificada!</h1>

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
											
												<p style="text-align: center;">Clique no botão abaixo para voltar.</p>

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
													<a class="mcnButton " title="Voltar" href=".." target="_self" style="font-weight: bold;letter-spacing: -0.5px;line-height: 100%;text-align: center;text-decoration: none;color: #FFFFFF;">Voltar</a>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
		