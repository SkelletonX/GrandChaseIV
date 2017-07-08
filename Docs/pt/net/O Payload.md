# **O Payload**

O que você vê abaixo é o payload decriptado do nosso pacote (agora com o padding removido).

> ![](http://i.imgur.com/nQlqmtm.png)

Como dito anteriormente, ele é o portador das informações mais importantes em todo o packet.

Assim como o buffer do pacote, o payload decriptado tem seus próprios segmentos: o _header_, o _conteúdo_ e um _preenchimento de bytes nulos_. Novamente vamos explicá-los um a um.
> Nota I: há algumas exceções para essa divisão, como, por exemplo, o packet de _ping_, que só possui bytes nulos _00_. 

> Nota II: diferentemente do **header do pacote**, os dados no **payload** são escritos no formato [big-endian](https://pt.wikipedia.org/wiki/Extremidade_(ordena%C3%A7%C3%A3o)).

## Header
> ![](http://i.imgur.com/C19kDWK.png)

O header do payload contém três informações essenciais: _ID_ do pacote, _tamanho do conteúdo_ e _indicador de compressão_. A seguir, nós daremos uma olhada mais de perto nesses valores.

### ID
> ![](http://i.imgur.com/JJfLbND.png)

O ID, como o nome sugere, é o identificador do pacote. Ele indica qual o propósito do packet, o que ele é.

Por exemplo, o pacote de ID 0x0001 é o pacote em que as chaves da sessão são definidas; o de ID 0x001C (que é o nosso caso) é o pacote de reconhecimento SHA_FILENAME_LIST, que serve para informar o cliente sobre os arquivos que serão verificados através de SHA checksum.

### Tamanho do Conteúdo
> ![](http://i.imgur.com/pTkORlB.png)

Esse é o tamanho em bytes do _conteúdo_ do payload. 

No nosso caso, ele é _00 00 00 40_ em valores hexadecimais, denotando que o tamanho é _64_ em decimal. Veja por si mesmo: conte cada byte do 1º após o header ao 4º último. Sua contagem deverá alcançar 64.

### Indicador de Compressão (_Compression Flag_)
> ![](http://i.imgur.com/OZSqBEU.png)

O indicador de compressão é um valor booleano que indica se o conteúdo está comprimido ou não. 

Quando o indicador é _verdadeiro_ (**01**), significa que os dados do conteúdo estão comprimidos. Caso contrário, ele é _falso_ (**00**) e indica que os dados não estão comprimidos, que é o caso do nosso pacote.

A compressão em si será discutida em breve.

## Conteúdo
> ![](http://i.imgur.com/EbaO45Q.png)

Basicamente, o conteúdo é a mensagem em sua forma mais pura. Na verdade, é a informação que o packet realmente deve transmitir.

Sua estrutura irá variar de packet para packet, mas há, ainda, um "padrão" comum. Tomando o valor "main.exe" como exemplo, veja o que segue.

> ![](http://image.prntscr.com/image/276d51bc2b4e4b2e820c1abefad4ab21.png)

A porção marcada em roxo é uma _string_ [unicode](https://pt.wikipedia.org/wiki/Unicode) que representa o nome do arquivo _main.exe_. Mas e quanto aos bytes em vermelho (_00 00 00 10_)? Eles são um inteiro de 4 bytes que representa o tamanho do valor que o sucede. Em decimal, _00 00 00 10_ é _16_, que é exatamente o tamanho da nossa string em bytes.

Mas lembre-se: podem haver valores que não são precedidos por seu tamanho!
## Preenchimento de Bytes Nulos (_Null Bytes Padding_)
> ![](http://i.imgur.com/XKdghFa.png)

É apenas um preenchimento composto por três bytes _00_ (nulos) ao fim de cada payload. 

## Compressão

Alguns dos pacotes do Grand Chase tem o conteúdo de seu payload comprimido.

Para comprimir dados, o Grand Chase usa a [zlib](https://pt.wikipedia.org/wiki/Zlib). Podemos dizer isso por conta da presença de um dos headers da zlib (_78 01_) em todos os payloads comprimidos.

Vamos dar uma olhada em um.
> ![](http://i.imgur.com/u51tXBH.png)

(Como você pode ver nos bytes em vermelho, o indicador de compressão é _verdadeiro_ e o header da zlib está presente)

Na verdade, apenas a parte destacada em roxo é comprimida: o header e os 4 primeiros bytes do conteúdo permanecem normais, bem como o preenchimento _00 00 00_ no final. 

Esses 4 bytes do conteúdo dos payloads comprimidos (no nosso caso, marcados de verde) são um número inteiro que representa o **tamanho da parte comprimida depois de descomprimida**. Note que, diferentemente dos outros dados do conteúdo, o segundo tamanho é little-endian, sendo _764_ em decimal.

Depois que os dados são descomprimidos, o payload pode ser lido normalmente como qualquer um não comprimido.

**Continue lendo**: [O Início da Sessão](./O%20Início%20da%20Sessão.md#o-início-da-sessão)
