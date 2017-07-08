# **A Criptografia**
> Nota: para um melhor entendimento, é altamente recomendado que você leia os conteúdos dos links fornecidos nesta seção.

Os payloads dos packets do Grand Chase são criptografados usando o [algoritmo DES](https://pt.wikipedia.org/wiki/Data_Encryption_Standard) através do [modo CBC](https://pt.wikipedia.org/wiki/Modo_de_opera%C3%A7%C3%A3o_(criptografia)#Modo_CBC_.28Cipher-block_chaining.29) (Cipher Block Chaining).

O processo de criptografia usa um _IV_, uma _chave_ de 8 bytes e um _plaintext_ (texto claro).
* Como dito anteriormente, um IV é gerado para cada pacote e é enviado junto com ele;
* A chave de criptografia é definida no início da sessão assim como a de autenticação;
* O plaintext é o payload não criptografado;

Pelo modo CBC, os dados são processados em blocos de 8 bytes cada. Mas e se o tamanho do nosso payload não for divisível por 8? É o que veremos abaixo.

### Padding
> ![](http://i.imgur.com/HR4kOAA.png)

O que você vê aí acima é o payload decriptado do nosso pacote, mas, por ora, vamos nos limitar apenas à parte destacada. 

Essa parte em vermelho é o _padding_. Ele serve para preencher os dados até que eles chegem a um determinado tamanho divisível pelo tamanho do bloco (no nosso caso, 8).

Tomemos nosso payload aqui como exemplo. Sem padding, seu tamanho seria 74, mas 74 não é divisível por 8. O próximo número divisível por 8 depois do 74 é 80, então o tamanho do nosso padding deve ser 6 (74 + 6 = 80). Assim, nós começamos a contar: 00, 01, 02, 03, 04 e 04 de novo. O último byte do padding é sempre igual ao penúltimo. 

Depois disso tudo, nós temos ***00 01 02 03 04 04***: um padding de 6 bytes.

### O Algoritmo do Padding

Como você pode ter imaginado, é impossível que o padding tenha seu último byte igual ao penúltimo se, por exemplo, seu tamanho fosse apenas 1. Vamos explicar o algoritmo um pouco melhor agora.

Na verdade é bem simples: quando a distância do tamanho do payload ao próximo número divisível por 8 for maior ou igual a 3, o comprimento do padding será essa distância. Quando for menor, será o tamanho do bloco (8) mais a distância.

Depois disso, o único passo restante é escrever os bytes. O trecho de código abaixo deve ser mais esclarecedor.
```C#
// Calcula a distância entre o tamanho e o próximo valor divisível por 8.
int distance = 8 - (dataLength % 8);

if (distance >= 3)
{
  paddingLength = distance;
}
else
{
  paddingLength = 8 + distance;
}
for (byte i = 0; i < (paddingLength - 1); i++)
{
  padding[i] = i;
}
padding[paddingLength - 1] = padding[paddingLength - 2];
```
E aqui está uma tabela com todos os 8 paddings possíveis para os payloads do jogo para acabar com qualquer dúvida restante:

| Distância          | Tamanho do Padding | Bytes do Padding                      |
| ------------------ | ------------------ | ------------------------------------- |
| 0                  | 8                  | ***00 01 02 03 04 05 06 06***         |
| 1                  | 9                  | ***00 01 02 03 04 05 06 07 07***      |
| 2                  | 10                 | ***00 01 02 03 04 05 06 07 08 08***   |
| 3                  | 3                  | ***00 01 01***                        |
| 4                  | 4                  | ***00 01 02 02***                     |
| 5                  | 5                  | ***00 01 02 03 03***                  |
| 6                  | 6                  | ***00 01 02 03 04 04***               |
| 7                  | 7                  | ***00 01 02 03 04 05 05***            |

**Continue lendo**: [O Payload](./O%20Payload.md#o-payload)
