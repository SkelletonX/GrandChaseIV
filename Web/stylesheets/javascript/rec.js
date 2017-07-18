REC = {
    url: false,

    init: function() { 
        this.rec();
    },

    rec: function() {
        self = this;

        $(document.rec).submit(function() {
            $('#ativar_pincode').fadeOut(500);

            setTimeout(function(){
                $('#retorno_ativar_pincode').fadeIn(600);
            },500);

            $.ajax({
                type: 'post',
                url: 'http://127.0.0.1/pbtroll2/functions/recuperar/',
                data: $(document.rec).serialize(),

                success: function(data) {
                    if (data == "Email enviado com sucesso!") {
                        $('#retorno_ativar_pincode .ico').removeClass('loading').addClass('sucesso');
                        $('#retorno_ativar_pincode .loader').append('<a style="text-decoration:none;color:orange;" href="javascript:location.reload();">Email enviado com sucesso!</a>');
						$('#retorno_ativar_pincode .loader').removeClass('loader').addClass('tente_novamente');
                    } else {
                        $('#retorno_ativar_pincode .ico').removeClass('loading').addClass('erro');
                        $('#retorno_ativar_pincode .loader').append('<a style="text-decoration:none;color:orange;" href="javascript:location.reload();">Tentar outra vez!</a>');
						$('#retorno_ativar_pincode .loader').removeClass('loader').addClass('tente_novamente');
					}

                    $('#retorno_ativar_pincode p').html(data);
                },

                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    $('#retorno_ativar_pincode .ico').removeClass('loading').addClass('atencao');
                    $('#retorno_ativar_pincode .loader').append('<a style="text-decoration:none;color:orange;" href="javascript:location.reload();">Tentar outra vez!</a>');
					$('#retorno_ativar_pincode .loader').removeClass('loader').addClass('tente_novamente');
                    $('#retorno_ativar_pincode p').html('Perda de Conexao.');
                },
            });
        });
    },
}

$(document).ready(function(){
    REC.init();
});