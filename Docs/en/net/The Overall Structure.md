# **The Overall Structure**
In Grand Chase, the packets are divided primarily into three sections: _header_, _payload_ and _authentication code_. Let's explain them one by one.

> For demonstration purposes, we will be using the acknowledgement packet SHA_FILENAME_LIST (ID: 0x001C).

If we had sniffed this packet, its buffer would look like this:

> ![](http://i.imgur.com/GiER0Di.png)

Now let's analyze its parts:
## Header
> ![](http://i.imgur.com/mNqvlYx.png)

In all packets, it represents the first 16 bytes of the received buffer. It contains some basic informations about the packet, which will be explained in detail below.
> Note: all the data in the header is written in the [little-endian](https://en.wikipedia.org/wiki/Endianness#Little-endian) format.

### Size
> ![](http://i.imgur.com/juPifVT.png)

As the name suggests, it represents the packet's data total length.

It is in the little-endian format, so it's actually _00 6A_, which is _106_ in decimal. If you count each byte of our buffer's data, you will notice that it contains exactly 106 bytes. :smiley:

### Prefix
> ![](http://i.imgur.com/9gVzt3M.png)

We're now faced with the *prefix*. 

It's represented by 2 random bytes generated at the beginning of the session. The exception is the packet in which the session keys are defined, where the prefix is always _00 00_, after all, it's inside it that is the new generated prefix (this packet will be explained in detail later).

Note that the generated prefix for the server's packets isn't the same as that used in the packets sent by the client.

### Count
> ![](http://i.imgur.com/B9v5VDh.png)

It's a 4-byte integer that represents the count of sent packets within a session. Note that both client and server have their own counts, that is, the client counts the packets sent by the client and the server counts the packets sent by the server.

In our case, the packet count is 2 since it's _00 00 00 02_ in hex with usual endianness.

Like the prefix, the count has as exception the same packet (as pointed before, this will be discussed later).

### IV (Initialization Vector)
> ![](http://i.imgur.com/pUd7n8j.png)

It's the IV used to encrypt the packet's payload.

For each packet is generated its own IV, which consists on 8 bytes equal ranging from _00_ to _FF_ in hex values. You should take a look at the [encryption section](./The%20Encryption.md#the-encryption) to have a better understanding of this concept.

## Payload (encrypted)
> ![](http://i.imgur.com/PEtA9jj.png)

Located between the 16 first (header) and the 10 last (auth code) bytes, this is the main part of the packet.

At first sight, it's encrypted and doesn't reveal much, but when decrypted, contains the effective data, the one that tell us something relevant such as the login inputted by the user or the information of the players inside a dungeon room.

Due to its importance, the payload will be discussed in its [own](./The%20Payload.md#the-payload) section and likewise will be the [encryption](./The%20Encryption.md#the-encryption).

## Authentication Code
> ![](http://i.imgur.com/iyWTNuP.png)

Represented by the last 10 bytes of the buffer, this is the portion of the packet which is meant to assure the authenticity and integrity of the rest. 

In Grand Chase, it consists in a [MD5](https://en.wikipedia.org/wiki/MD5)-[HMAC](https://en.wikipedia.org/wiki/Hash-based_message_authentication_code) (Hash-based Message Authentication Code). 

> ![](http://i.imgur.com/G7wV9BW.png)

The authentication code calculation is done based on the portion of the packet's buffer shown above (from the first byte after the packet size until the last byte of the encrypted payload). The calculation also takes an 8-byte auth key which is defined at the beginning of the networking session (it will be better detailed in the [last section](./The%20Beginning%20of%20the%20Session.md#the-beginning-of-the-session)).

Normally, a MD5-HMAC would have a size of 16 bytes. But if we take a look at our packet's HMAC we will notice that it's only 10 bytes long. That's because in Grand Chase is truncated, being left with the size of 10 bytes.
> ![](http://i.imgur.com/uTFcywp.png)

In red, you can see the portion of the HMAC present in the packet compared to the entire HMAC calculated to that section of the packet's data.

**Further reading**: [The Encryption](./The%20Encryption.md#the-encryption)
