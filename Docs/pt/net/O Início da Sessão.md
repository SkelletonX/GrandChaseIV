# **O Início da Sessão**

Nesta seção, estaremos discutindo algo muito importante: o início de uma sessão de rede de Grand Chase. Mais especificamente, trataremos do packet onde as chaves são definidas.

No Grand Chase, as chaves de criptografia e de autenticação para uma sessão são geradas aleatoriamente pelo servidor e enviadas para o cliente através do pacote de ID 0x0001, que é o primeiro pacote com alguma informação enviado.

Seus dados se pareceriam com estes:

> Nota: o packet aqui exposto é da season eternal. Eu não verifiquei os das outras seasons, mas eles podem ser diferentes.

--
> ![](http://i.imgur.com/jD40Gtt.png)

Antes de tudo, vamos falar sobre dois aspectos: o _prefixo_ e a _contagem_.

* **Prefixo**: no "pacote inicial", o prefixo não é aleatório. Ao invés disso, é sempre _00 00_. O novo prefixo é gerado pelo servidor e enviado no conteúdo desse pacote;
* **Contagem**: no "pacote inicial", (obviamente) a contagem não mede a quantidade de pacotes enviados. Nesse caso, a contagem é a de sessões iniciadas em um servidor, ou seja, o número de conexões já realizadas;

Você pode estar pensando: "Se as chaves da sessão estão dentro do payload encriptado, quais chaves foram usadas para criptografar os dados e gerar o HMAC desse pacote?"

Para esse packet, o Grand Chase usa duas chaves padrões que são armazenadas por ambos servidor e cliente:

* **Chave de Criptografia Padrão:** C7 D8 C4 BF B5 E9 C0 FD
* **Chave de Autenticação Padrão:** C0 D3 BD C3 B7 CE B8 B8

Tendo isso explicado, agora vamos analisar o payload do pacote.
> ![](http://i.imgur.com/QMBOl73.png)

Ele é como o de qualquer outro: tem um header, um conteúdo, e um preenchimento de bytes nulos ao final. Foquemos o conteúdo.

Os valores destacados são, respectivamente, o _prefixo_ a ser utilizado nos pacotes seguintes enviados pelo servidor e as _chaves_ de _autenticação_ e de _criptografia_ definidas para o resto da sessão de rede. No nosso caso:

* **A parte em verde é o novo prefixo**: 8E E7
* **A parte em roxo é a chave de autenticação**: 62 88 F3 A7 D3 2C 87 C5
* **A parte em vermelho é a chave de criptografia**: A4 29 1C 74 B2 CE 4A 34

Se voltarmos ao pacote que observamos no início do tutorial, podemos ver que [seu prefixo](./A%20Estrutura%20Geral.md#prefixo) (_E7 8E_) corresponde ao definido no "pacote inicial" (_8E E7_). O detalhe é que o prefixo está em little-endian no header, enquanto no conteúdo do "pacote inicial" ele está em big-endian.


E, por enquanto, isso é tudo :smile:
