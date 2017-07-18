PIN = {
    url: false,
    caracteres: 1,

    init: function() {
        this.apenasNumeros();
        this.contaCaracteres();  
        this.ativaPINCode();
    },

    ativaPINCode: function() {
        self = this;

        $(document.pincode).submit(function() {
         if (self.caracteres < 16 || self.caracteres > 20) return false;
            $('#ativar_pincode').fadeOut(500);

            setTimeout(function(){
                $('#retorno_ativar_pincode').fadeIn(600);
            },500);

            $.ajax({
                type: 'post',
                url: self.url,
                data: $(document.pincode).serialize(),

                success: function(data) {
                    if (data == "Pin-Code ativado com sucesso") {
                        $('#retorno_ativar_pincode .ico').addClass('sucesso');
                        $('#retorno_ativar_pincode .loader').append('<a style="text-decoration:none;color:orange;" href="javascript:location.reload();">Ativar outro Cartao!</a>');
						$('#retorno_ativar_pincode .loader').removeClass('loader');
                    } else {
                        $('#retorno_ativar_pincode .ico').addClass('erro');
                        $('#retorno_ativar_pincode .loader').append('<a style="text-decoration:none;color:orange;" href="javascript:location.reload();">Tentar outra vez!</a>');
						$('#retorno_ativar_pincode .loader').removeClass('loader').addClass('tente_novamente');
					}

                    $('#retorno_ativar_pincode p').html(data);
                },

                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    $('#retorno_ativar_pincode .ico').addClass('atencao');
                    $('#retorno_ativar_pincode .loader').append('<a style="text-decoration:none;color:orange;" href="javascript:location.reload();">Tentar outra vez!</a>');
					$('#retorno_ativar_pincode .loader').removeClass('loader').addClass('tente_novamente');
                    $('#retorno_ativar_pincode p').html('Perda de Conexao.');
                },
            });
        });
    },

    contaCaracteres: function() {
        self = this;

        $('#pin').keyup(function () { 
            self.caracteres = this.value.length;

            if (self.caracteres == 16) self.url = 'http://127.0.0.1/pbtroll2/functions/cash/';

            if (self.caracteres == 16) $(".box_captcha").slideDown();
            else $(".box_captcha").slideUp();
        });
    },

    apenasNumeros: function() {
        $('#pin').keyup(function () { 
            this.value = this.value.replace(/[^0-9\.]/g,'');
        });
    },
}

$(document).ready(function(){
    PIN.init();
});

function sleep(milliseconds) {
	var start = new Date().getTime();
		for (var i = 0; i < 1e7; i++) { if ((new Date().getTime() - start) > milliseconds){
			break;
		}
	}
}