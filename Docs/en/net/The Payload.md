# **The Payload**

What you see below is the decrypted payload of our packet (now with the padding removed).

> ![](http://i.imgur.com/nQlqmtm.png)

As previously stated, it's the holder of the most important data in all the packet.

Like the packet buffer, the decrypted payload has its sections: the _header_, the _content_ and the _null bytes padding_. Again, let's explain them one by one.
> Note I: there are some exceptions to this division like the ping packet, whose payload contains only null bytes.

> Note II: unlike the **packet's header**, all the data in the **payload** is written in the [big-endian](https://en.wikipedia.org/wiki/Endianness#Big-endian) format.

## Header
> ![](http://i.imgur.com/C19kDWK.png)

The payload's header contains three essential informations: packet _ID_, _content size_ and _compression flag_. Next, we will take a closer look at these values.

### ID
> ![](http://i.imgur.com/JJfLbND.png)

The ID, as the name suggests, is the packet identifier. It indicates what the packet is meant for, what it is. 

For example, the packet with the ID 0x0001 is the packet in which the session keys are defined; the one with the ID 0x001C is the acknowledgement packet SHA_FILENAME_LIST, which serves to inform the client about the files which will be verified through SHA checksum.

### Content Size
> ![](http://i.imgur.com/pTkORlB.png)

This is the size in bytes of the _content_ of the payload.

In our case, it is _00 00 00 40_ in hex values, denoting that the size is _64_ in decimal. Check for yourself: count each byte from the 1st after the header to the 4th last byte. Your count should reach 64.

### Compression Flag
> ![](http://i.imgur.com/OZSqBEU.png)

The _compression flag_ is a boolean value that indicates whether the content data is compressed or not.

When it is _true_ (**01**), it means the data is compressed. Otherwise, it is _false_ (**00**) and indicates that the data is uncompressed, which is the case of our packet. 

The compression itself will be discussed soon.

## Content
> ![](http://i.imgur.com/EbaO45Q.png)
  
Basically, the content is the message in its rawest state. It's actually the information that the packet is really intended to transmit.

Its structure will vary for each packet type, but there's yet one common "pattern". Taking the "main.exe" value as example, see what follows.

> ![](http://image.prntscr.com/image/276d51bc2b4e4b2e820c1abefad4ab21.png)
  
The portion marked in purple is an [unicode](https://en.wikipedia.org/wiki/Unicode) string that represents the filename _main.exe_. But what about the piece in red (_00 00 00 10_)? They're a 4-byte integer that represents the size of the following value. In decimal, _00 00 00 10_ is _16_, which is exactly our string's size in bytes.

But remember: there may be values that aren't preceded by its size!
## Null Bytes Padding
> ![](http://i.imgur.com/XKdghFa.png)

It is just an ordinary padding composed by three _00_ (null) bytes at the end of each payload.

## Compression

Some of the Grand Chase's packets have their payload content compressed. 

To compress the data, Grand Chase uses [zlib](https://en.wikipedia.org/wiki/Zlib). We can know this because of the presence of one of the zlib headers (_78 01_) in every payload which have its data compressed.

Let's take a look at one compressed payload.
> ![](http://i.imgur.com/u51tXBH.png)

(As you can see in the bytes in red, the compression flag is _true_ and the zlib header is present)

Actually, only the portion highlighted with purple is compressed: the header and the first 4 bytes of the content remains uncompressed, as well as the _00 00 00_ padding at the end.

These 4 bytes from the content of the compressed payloads (in our case, marked in green) are an integer which represents the **size of the compressed portion after decompressed**. Note that, exceptionally, this second size is little-endian, being _764_ in decimal.

After the data is decompressed, the payload can be read normally like any uncompressed other.

**Further reading**: [The Beginning of the Session](./The%20Beginning%20of%20the%20Session.md#the-beginning-of-the-session)
