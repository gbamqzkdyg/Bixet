# Bixet：应用层数据解析工具集
---
### 本文内容
>#### [1. 项目描述](#项目描述)
>>##### [1.1. 项目介绍](#项目介绍)
>>##### [1.2. 项目依赖](#项目依赖)
>>>###### [1.2.1. 运行环境](#运行环境)
>>>###### [1.2.2. 项目依赖命名空间](#项目依赖命名空间)
>>##### [1.3. 使用方法](#使用方法)
>>##### [1.4. 数据内存布局](#数据内存布局)
>#### [2. 枚举类](#枚举类)
>#### [3. BUtil类](#BUtil类)
>>##### [3.1 类内常量](#BUtil类内常量)
>>##### [3.1 方法](#BUtil方法)
>#### [4. BReader类](#BReader类)
>>##### [4.1 示例](#BReader示例)
>>##### [4.2 构造函数](#BReader构造函数)
>>##### [4.3 类内常量](#BReader类内常量)
>>##### [4.4 属性](#BReader属性)
>>##### [4.5 运算符重载](#BReader运算符重载)
>>##### [4.6 方法](#BReader方法)
>#### [5. BWriter类](#BWriter类)
>>##### [5.1 示例](#BWriter示例)
>>##### [5.2 构造函数](#BWriter构造函数)
>>##### [5.3 类内常量](#BWriter类内常量)
>>##### [5.4 属性](#BWriter属性)
>>##### [5.5 运算符重载](#BWriter运算符重载)
>>##### [5.6 方法](#BWriter方法)
---
## 项目描述
### 项目介绍
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bixet是一组工具集合，为用户提供对以字节流或比特流形式表示的数据进行操作的若干功能。不同的功能被实现为不同的类以供用户使用。本项目计划支持的功能及其最新实现版本如下所示：

|类名|功能|最新实现版本|
|-|-|-|
|__BUtil__|对字节流或比特流进行操作的工具方法集|0.2.0|
|__BReader__|从数据指定字节位置/比特位置处依照指定字节序/比特序读取指定长度的数值或字符串|0.4.0|
|__BWriter__|向数据指定字节位置或比特位置处依照指定字节序/比特序写入指定长度的数值或字符串|0.4.2|
|__BTemplete__ & __BBlock__ & __BVariable__|将数据格式描述为形式化模板|开发中|
|__BResolver__|依照数据格式模板对数据进行自动化的解析与填充|待实现|
###### [<p align="right">返回目录</p>](#本文内容)
***
### 项目依赖
### 运行环境
    .Net Framework 4.7.2及以上版本
###### [<p align="right">返回目录</p>](#本文内容)
***
### 项目依赖命名空间

    1. Newtonsoft.Json
    2. FluentAssertions
    3. Microsoft.VisualStudio.TestTools.UnitTesting
###### [<p align="right">返回目录</p>](#本文内容)
***
### 使用方法
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在.Net项目中引用编译后的**Bixet.dll**，并在自己程序中需要的位置添加对命名空间的使用。
###### [<p align="right">返回目录</p>](#本文内容)
***
### 数据内存布局
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bixet中默认的数据的字节排布顺序与字节内部的比特排布顺序如下图所示：

![内存布局](Items/MemoryLayout.png "内存布局")

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;即对于下述代码所描述的数据：
```C#
byte[] data = new byte[]{ 0xEF, 0x12, 0x34, 0x56, 0x78, 0xFE};
```
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bixet内全部工具将认为数据的字节1至字节4依次为0x12, 0x34, 0x56, 0x78；数据的比特17至比特23分别为0、1、0、1、1、0、0。

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;若用户数据的内存布局与Bixet使用布局不一致，用户可使用Bixet所提供的工具方法对数据内存布局进行转换。
###### [<p align="right">返回目录</p>](#本文内容)
***

## BUtil类
<div style="font-size:70%">命名空间：Bixet</div>
<div style="font-size:70%">程序集：Bixet.dll</div>

</br>
对比特数据或字节数据进行操作的工具方法集合静态类。

```C#
public static class BUtil
```

### BUtil类内常量
|常量名|说明|
|-|-|
|**version**|BUtil的版本号|
###### [<p align="right">返回目录</p>](#本文内容)

### BUtil方法
|方法签名|说明|
|-|-|
|public static _void_ _**ReverseByteEndian**_(_byte[]_ **bytes**)|逆转**byte**中全部字节的字节序|
|public static _void_ _**ReverseByteEndian**_(_byte[]_ **bytes**, int **begin**, int **end**)|逆转**bytes**的第**begin**个字节至第**end**个字节的字节序|
|public static _void_ _**ReverseBitEndian**_(_byte[]_ **bytes**)|逆转**bytes**的每个字节的比特序|
|public static _void_ _**ReverseBitEndian**_(_System.Collections.BitArray_ **bits**)|按照8比特1组逆转**bits**中全部各组比特的比特序</br>_注：**bits**的长度必须为8的整数倍_|
|public static _void_ _**ReverseBitEndian**_(_System.Collections.BitArray_ **bits**, _int_ **begin**, _int_ **end**)|按照8比特1组逆转**bits**中从第**begin**至第**end**个比特中的全部各组比特的比特序</br>_注：**end** - **begin** 的长度必须为8的整数倍|
|public static _void_ _**ReverseBitsOrder**_(_byte[]_ **bytes**)|逆转**bytes**全部比特的顺序|
|public static _void_ _**ReverseBitsOrder**_(_System.Collections.BitArray_ **bits**)|逆转**bits**全部比特的顺序|
|public static _void_ _**ReverseBitsOrder**_(_System.Collections.BitArray_ **bits**, _int_ **begin**, _int_ **end**)|逆转**bits**的第**begin**个比特至第**end**个比特的顺序|
###### [<p align="right">返回目录</p>](#本文内容)

## 枚举类
<div style="font-size:70%">命名空间：Bixet</div>
<div style="font-size:70%">程序集：Bixet.dll</div>
</br>
用于描述数据及数据结构性质的枚举类。

|类名|说明|枚举值|
|-|-|-|
|**Endian**|字节或比特的排布顺序|**BigEndian**: 大端序</br>**SmallEndian**:小端序|
###### [<p align="right">返回目录</p>](#本文内容)

## BReader类
<div style="font-size:70%">命名空间：Bixet</div>
<div style="font-size:70%">程序集：Bixet.dll</div>

</br>
从字节数组的指定位置中读取数值或字符串。

```C#
public class BReader
```
###### [<p align="right">返回目录</p>](#本文内容)

### BReader示例
```C#
using System;
using Bixet;

class Program
{
    static void Main()
    {
        byte[] data = new byte[] { 
            0x01, 0x12, 0x34, 0x56, 0x78, 0b10101010, 0b11100100, 
            (byte)'B', (byte)'i', (byte)'x', (byte)'e', (byte)'t'
        };
        BReader br = new BReader(data);
        byte aByte = br.ReadValueByByteIndex<byte>(0, 1);
        Console.WriteLine($"Read a byte from the 0th byte :" +
            $"0x{Convert.ToString(aByte, 16).PadLeft(2, '0')}");
        int aInt = br.ReadValueByByteIndex<int>(1, 4);
        Console.WriteLine($"Read an integer from the 1th-4th bytes: " +
            $"0x{Convert.ToString(aInt, 16).PadLeft(8, '0')}");
        for (int i = 0; i < 8; ++i)
        {
            Console.WriteLine($"Read a bit from the {i}th bit of the 5th byte: " +
                $"{br.ReadValueByBitIndex<bool>(5, i, 1)}");
            Console.WriteLine($"The above line is equivalent to read a bit from the {40 + i}th bit: " +
                $"{br.ReadValueByBitIndex<bool>(40 + i, 1)}");
        }
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Read a byte from the {2 * i}-{2 * i + 1}th bits of the 6th byte :" +
                $"0x{Convert.ToString(br.ReadValueByBitIndex<byte>(6, 2 * i, 2), 16).PadLeft(2, '0')}");
            Console.WriteLine($"The above line is equivalent to read a byte from the {48 + 2 * i}-{48 + 2 * i + 1} bits: " +
                $"0x{Convert.ToString(br.ReadValueByBitIndex<byte>(48 + 2 * i, 2), 16).PadLeft(2, '0')}");
        }
        Console.WriteLine($"Read a byte from the 8 bits beginning at the 4th bit of 5th byte: " +
            $"0x{Convert.ToString(br.ReadValueByBitIndex<byte>(5, 4, 8), 16).PadLeft(2, '0')}");
        Console.WriteLine($"The above line is equivalent to read a byte from the 8 bits beginning at the 44th bit: " +
            $"0x{Convert.ToString(br.ReadValueByBitIndex<byte>(44, 8), 16).PadLeft(2, '0')}");
        Console.WriteLine($"Read a 5 bytes long string with length 5 from the 7th byte :" +
            $"{br.ReadStringByByteIndex(7, 5)}");
        Console.WriteLine($"The above line is equivalent to read a 40 bits long string from the 0th bit of the 7th byte: " +
            $"{br.ReadStringByByteIndex(7, 5)}");
        Console.WriteLine($"The above line is equivalent to read a 40 bits long string from the 56th bit: " +
            $"{br.ReadStringByByteIndex(7, 5)}");
        Console.ReadKey();

        /* Output:
        Read a byte from the 0th byte: 0x01
        Read an integer from the 1th-4th bytes: 0x12345678
        Read a bit from the 0th bit of the 5th byte: False
        The above line is equivalent to read a bit from the 40th bit: False
        Read a bit from the 1th bit of the 5th byte: True
        The above line is equivalent to read a bit from the 41th bit: True
        Read a bit from the 2th bit of the 5th byte: False
        The above line is equivalent to read a bit from the 42th bit: False
        Read a bit from the 3th bit of the 5th byte: True
        The above line is equivalent to read a bit from the 43th bit: True
        Read a bit from the 4th bit of the 5th byte: False
        The above line is equivalent to read a bit from the 44th bit: False
        Read a bit from the 5th bit of the 5th byte: True
        The above line is equivalent to read a bit from the 45th bit: True
        Read a bit from the 6th bit of the 5th byte: False
        The above line is equivalent to read a bit from the 46th bit: False
        Read a bit from the 7th bit of the 5th byte: True
        The above line is equivalent to read a bit from the 47th bit: True
        Read a byte from the 0-1th bits of the 6th byte: 0x00
        The above line is equivalent to read a byte from the 48-49 bits:  0x00
        Read a byte from the 2-3th bits of the 6th byte: 0x01
        The above line is equivalent to read a byte from the 50-51 bits:  0x01
        Read a byte from the 4-5th bits of the 6th byte: 0x02
        The above line is equivalent to read a byte from the 52-53 bits:  0x02
        Read a byte from the 6-7th bits of the 6th byte: 0x03
        The above line is equivalent to read a byte from the 54-55 bits:  0x03
        Read a byte from the 8 bits beginning at the 4th bit of 5th byte: 0x4a
        The above line is equivalent to read a byte from the 8 bits beginning at the 44th bit: 0x4a
        Read a 5 bytes long string with length 5 from the 7th byte: Bixet
        The above line is equivalent to read a 40 bits long string from the 0th bit of the 7th byte: Bixet
        The above line is equivalent to read a 40 bits long string from the 56th bit: Bixet
        */
    }
}
```
###### [<p align="right">返回目录</p>](#本文内容)

### BReader构造函数
|构造函数签名|说明|
|-|-|
|public _**BReader**_(_byte[]_ **bytes**)|以**bytes**的全部字节作为可读取数据|
|public _**BReader**_(_byte[]_ **bytes**, _int_ **length**)|以**bytes**的前**length**个字节作为可读取数据|
|public _**BReader**_(_byte[]_ **bytes**, _int_ **offset**, _int_ **length**)|以**bytes**中从**offset**个字节开始的**length**个字节作为可读取数据|
###### [<p align="right">返回目录</p>](#本文内容)

### BReader类内常量
|常量名|说明|
|-|-|
|public const _string_ **version**|BReader的版本号|
|public const _int_ **maxBytesLength**|单次可读取的最大字节数|
|public const _int_ **maxBitsLength**|单次可读取的最大比特数|
###### [<p align="right">返回目录</p>](#本文内容)

### BReader属性
|属性名|说明|
|-|-|
|public _int_ **BytesCount**|可读取数据字节数|
|public _int_ **BitsCount**|可读取数据比特数|
###### [<p align="right">返回目录</p>](#本文内容)

### BReader运算符重载
|运算符签名|说明|
|-|-|
|public _byte_ _**this**_[_int_ **i**]|获取可读取数据的第**i**个字节|
|public _byte_ _**this**_[_int_ **i**, _int_ **j**]|获取可读取数据的第**i**个字节的第**j**个比特（结果以字节表示）|
###### [<p align="right">返回目录</p>](#本文内容)

### BReader方法
|方法签名|说明|
|-|-|
|public _byte[]_ _**GetRawBytes**_(_int_ **beginIndex**, _int_ **length**)|获取可读取数据从第**beginIndex**个字节处开始的**length**个字节|
|public _System.Collections.BitArray_ _**GetRawBits**_(_int_ **beginIndex**, _int_ **length**)|获取可读取数据从第**beginIndex**个比特处开始的**length**个比特|
|public _System.Collections.BitArray_ _**GetRawBits**_(_int_ **byteIndex**, _int_ **bitIndex**, _int_ **length**)|获取可读取数据从第**byteIndex**个字节的第**bitIndex**个比特处开始的**length**个比特|
|public _T_ _**ReadValueByByteIndex**_&#60;_T_&#62;(_int_ **beginIndex**, _int_ **length**, _Endian_ **byteEndian** = _Endian_.BigEndian)|将可读取数据的从第**beginIndex**个字节开始的**length**个字节按照**byteEndian**的字节序读取为_T_类型的数值</br>_注：支持读取的数值的字节数不能超过**maxBytesLength**，支持读取的类型为：**sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public **string** _**ReadStringByByteIndex**_(_int_ **beginIndex**, _int_ **length**, _Endian_ **byteEndian** = _Endian_.BigEndian, _System.Text.Encoding_ **encoding** = null)|将可读取数据的从第**beginIndex**个字节开始的**length**个字节按照**byteEndian**的字节序以**encoding**编码方式读取为字符串</br>_注：若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
|public _T_ _**ReadValueByBitIndex**_&#60;_T_&#62;(_int_ **beginIndex**, _int_ **length**, _Endian_ **bitEndian** = _Endian_.SmallEndian)|将可读取数据的从第**beginIndex**个比特开始的**length**个比特按照**bitEndian**的比特序读取为_T_类型的数值</br>_注：支持读取的数值的比特数不能超过**maxBitsLength**，支持读取的类型为：**bool**, **sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public _T_ _**ReadValueByBitIndex**_&#60;_T_&#62;(_int_ **byteIndex**, _int_ **bitIndex**, _int_ **length**, _Endian_ **bitEndian** = _Endian_.SmallEndian)|将可读取数据的从第**byteIndex**个字节的第**bitIndex**个比特开始的**length**个比特按照**bitEndian**的比特序读取为_T_类型的数值</br>_注：支持读取的数值的最大比特数与支持类型与前一方法相同_|
|public **string** _**ReadStringByBitIndex**_(_int_ **beginIndex**, _int_ **length**, _Endian_ **bitEndian** = _Endian_.SmallEndian, _System.Text.Encoding_ **encoding** = null)|将可读取数据的从第**beginIndex**个比特开始的**length**个比特按照**bitEndian**的比特序以**encoding**编码方式读取为字符串</br>_注：输入的**length**参数长度必须为8的整数倍；若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
|public **string** _**ReadStringByBitIndex**_(_int_ **byteIndex**, _int_ **bitIndex**, _int_ **length**, _Endian_ **bitEndian** = _Endian_.SmallEndian, _System.Text.Encoding_ **encoding** = null)|将可读取数据的从第**byteIndex**个字节的第**bitIndex**个比特开始的**length**个比特按照**bitEndian**的比特序以**encoding**编码方式读取为字符串</br>_注：输入的**length**参数长度必须为8的整数倍；若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
###### [<p align="right">返回目录</p>](#本文内容)

## BWriter类
<div style="font-size:70%">命名空间：Bixet</div>
<div style="font-size:70%">程序集：Bixet.dll</div>

</br>
生成指定长度的字节数组，并向其写入数值或字符串。

```C#
public class BWriter
```
###### [<p align="right">返回目录</p>](#本文内容)

### BWriter示例
```C#
using System;
using Bixet;

class Program
{
    static void Main()
    {
        Console.WriteLine("Create a BWriter with length of 29 bytes");
        BWriter bw = new BWriter(29);
        Console.WriteLine("Write 4 bytes 0x12345678 starting at the 0th byte");
        bw.WriteValueByByteIndex<int>(0, 0x12345678, 4);
        Console.WriteLine("Write 32 bits 0x12345678 starting at the 32nd bit");
        bw.WriteValueByBitIndex<int>(32, 0x12345678, 32);
        Console.WriteLine("Write 32 bits 0x12345678 starting at the 0th bit of the 8th byte");
        bw.WriteValueByBitIndex<long>(8, 0, 0x12345678, 32);
        Console.WriteLine("Write 1 * 8 bits starting at the 0th bit of the 12th byte");
        for (int i = 0; i < 8; ++i)
        {
            bw.WriteValueByBitIndex<bool>(12, i, i % 2 == 1, 1);
        }
        Console.WriteLine("Write 2 * 4 bits starting at the 104th bit");
        for (short i = 0; i < 4; ++i)
        {
            bw.WriteValueByBitIndex<short>(104 + 2 * i, i, 2);
        }
        string s = "Bixet";
        Console.WriteLine("Write the 5-bytes-long string starting at the 14th byte");
        bw.WriteStringByByteIndex(14, s, 5);
        Console.WriteLine("Write the 40-bits-long string starting at the 152th bit");
        bw.WriteStringByBitIndex(152, s, 40);
        Console.WriteLine("Write the 40-bits-long string starting at the 0th bit of the 24th byte");
        bw.WriteStringByBitIndex(24, 0, s, 40);
        byte[] bytes = bw.GetData();
        Console.WriteLine($"Length of the generated bytes: {bytes.Length}");
        Console.Write("The first 4 bytes of the generated bytes: ");
        for (int i = 0; i < 4; ++i) Console.Write($"0x{Convert.ToString(bytes[i], 16)} ");
        Console.WriteLine();
        Console.Write("The 4th-7th bytes of the generated bytes: ");
        for (int i = 4; i < 8; ++i) Console.Write($"0x{Convert.ToString(bytes[i], 16)} ");
        Console.WriteLine();
        Console.Write("The 8th-11th bytes of the generated bytes: ");
        for (int i = 8; i < 12; ++i) Console.Write($"0x{Convert.ToString(bytes[i], 16)} ");
        Console.WriteLine();
        Console.WriteLine($"The 12th byte of the generated bytes: 0b{Convert.ToString(bytes[12], 2)}");
        Console.WriteLine($"The 13th byte of the generated bytes: 0b{Convert.ToString(bytes[13], 2)}");
        Console.WriteLine($"Decoded string from 14th-18th bytes of the generated bytes: {System.Text.Encoding.Default.GetString(bytes, 14, 5)}");
        Console.WriteLine($"Decoded string from 19th-23th bytes of the generated bytes: {System.Text.Encoding.Default.GetString(bytes, 19, 5)}");
        Console.WriteLine($"Decoded string from 24th-28th bytes of the generated bytes: {System.Text.Encoding.Default.GetString(bytes, 24, 5)}");
        Console.ReadKey();

        /* Output:
        Create a BWriter with length of 29 bytes
        Write 4 bytes 0x12345678 starting at the 0th byte
        Write 32 bits 0x12345678 starting at the 32nd bit
        Write 32 bits 0x12345678 starting at the 0th bit of the 8th byte
        Write 1 * 8 bits starting at the 0th bit of the 12th byte
        Write 2 * 4 bits starting at the 104th bit
        Write the 5-bytes-long string starting at the 14th byte
        Write the 40-bits-long string starting at the 152th bit
        Write the 40-bits-long string starting at the 0th bit of the 24th byte
        Length of the generated bytes: 29
        The first 4 bytes of the generated bytes: 0x12 0x34 0x56 0x78
        The 4th-7th bytes of the generated bytes: 0x78 0x56 0x34 0x12
        The 8th-11th bytes of the generated bytes: 0x78 0x56 0x34 0x12
        The 12th byte of the generated bytes: 0b10101010
        The 13th byte of the generated bytes: 0b11100100
        Decoded string from 14th-18th bytes of the generated bytes: Bixet
        Decoded string from 19th-23th bytes of the generated bytes: Bixet
        Decoded string from 24th-28th bytes of the generated bytes: Bixet
        */
    }
}

```
###### [<p align="right">返回目录</p>](#本文内容)

### BWriter构造函数
|构造函数签名|说明|
|-|-|
|public _**BWriter**_(_int_ **byteLength**)|设置可写入数据大小为**byteLength**字节|
###### [<p align="right">返回目录</p>](#本文内容)

### BWriter类内常量
|常量名|说明|
|-|-|
|public const _string_ **version**|BWriter的版本号|
|public const _int_ **maxBytesLength**|单次可写入的最大字节数|
|public const _int_ **maxBitsLength**|单次可写入与的最大比特数|
###### [<p align="right">返回目录</p>](#本文内容)

### BWriter属性
|属性名|说明|
|-|-|
|public _int_ **BytesCount**|可写入数据字节数|
|public _int_ **BitsCount**|可写入数据比特数|
###### [<p align="right">返回目录</p>](#本文内容)

### BWriter运算符重载
|运算符签名|说明|
|-|-|
|public _byte_ _**this**_[_int_ **i**]|写入第**i**个字节|
|public _byte_ _**this**_[_int_ **i**, _int_ **j**]|写入第**i**个字节的第**j**个比特（输入比特值以字节表示）|
###### [<p align="right">返回目录</p>](#本文内容)

### BWriter方法
|方法签名|说明|
|-|-|
|public _void_ _**SetRawBytes**_(_int_ **destIndex**, _byte[]_ **bytes**)|向可写入数据的第**destIndex**字节位置写入**bytes**的全部字节|
|public _void_ _**SetRawBytes**_(_int_ **destIndex**, _byte[]_ **bytes**, _uint_ **offset**, _int_ **length**)|向可写入数据的第**destIndex**字节位置写入**bytes**的从**offset**位置开始的**length**个字节|
|public _void_ _**SetRawBits**_(_int_ **destIndex**, _System.Collections.BitArray_ **bits**)|向可写入数据的第**destIndex**个比特位置写入**bits**的全部比特|
|public _void_ _**SetRawBits**_(_int_ **destIndex**, _System.Collections.BitArray_ **bits**, _int_ **offset**, _int_ **length**)|向可写入数据的第**destIndex**个比特位置写入**bits**从**offset**位置开始的**length**个比特|
|public _void_ _**SetRawBits**_(_int_ **byteIndex**, _int_ **bitIndex**, _System.Collections.BitArray_ **bits**)|向可写入数据的第**byteIndex**个字节的**bitIndex**个比特位置写入**bits**的全部比特|
|public _void_ _**SetRawBits**_(_int_ **byteIndex**, _int_ **bitIndex**, _System.Collections.BitArray_ **bits**, _int_ **offset**, _int_ **length**)|向可写入数据的第**byteIndex**个字节的**bitIndex**个比特位置写入**bits**从**offset**位置开始的**length**个比特|
|public _void_ _**WriteValueByByteIndex**_&#60;_T_&#62;(_int_ **beginIndex**, _T_ **value**, _Endian_ **byteEndian** = _Endian_.BigEndian)|向可写入数据的第**beginIndex**个字节处按照**byteEndian**的字节序写入类型为*T*的数据**value**</br>_注：写入的字节数为*T*所占用的字节数。支持写入的类型为：**sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public _void_ _**WriteValueByByteIndex**_&#60;_T_&#62;(_int_ **beginIndex**, _T_ **value**, _int_ **length**, _Endian_ **byteEndian** = _Endian_.BigEndian)|向可写入数据的第**beginIndex**个字节处按照**byteEndian**的字节序写入类型为*T*的数据**value**的前**length**个字节</br>_注：支持写入的类型为：**sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public _void_ _**WriteValueByBitIndex**_&#60;_T_&#62;(_int_ **beginIndex**, _T_ **value**, _int_ **length**, _Endian_ **bitEndian** = _Endian_.SmallEndian)|向可写入数据的第**beginIndex**个比特处按照**bitEndian**的比特序写入类型为*T*的数据**value**的前**length**个比特</br>_注：支持写入的类型为：**bool**, **sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public _void_ _**WriteValueByBitIndex**_&#60;_T_&#62;(_int_ **byteIndex**, _int_ **bitIndex**, _T_ **value**, _int_ **length**, _Endian_ **bitEndian** = _Endian_.SmallEndian)|向可写入数据的第**byteIndex**个字节的第**bitIndex**个比特处按照**bitEndian**的比特序写入类型为*T*的数据**value**的前**length**个比特</br>_注：支持写入的类型为：**bool**, **sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public _void_ _**WriteStringByByteIndex**_(_int_ **beginIndex**, _string_ **s**, _int_ **length**, _Endian_ **byteEndian** = _Endian_.BigEndian, _System.Text.Encoding_ **encoding** = null)|向可写入数据的第**beginIndex**个字节位置按照**encoding**的编码方式写入长度为**length**字节的字符串**s**</br>_注：若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
|||
|||
|||





|public _System.Collections.BitArray_ GetRawBits(int **beginIndex**, int **length**)|获取可读取数据从第**beginIndex**个比特处开始的**length**个比特|
|public _System.Collections.BitArray_ GetRawBits(int **byteIndex**, int **bitIndex**, int **length**)|获取可读取数据从第**byteIndex**个字节的第**bitIndex**个比特处开始的**length**个比特|
|public _T_ ReadValueByByteIndex&#60;_T_&#62;(int **beginIndex**, int **length**, Endian **byteEndian** = Endian.BigEndian)|将可读取数据的从第**beginIndex**个字节开始的**length**个字节按照**byteEndian**的字节序读取为_T_类型的数值</br>_注：支持读取的数值的字节数不能超过**maxBytesLength**，支持读取的类型为：**sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public **string** ReadStringByByteIndex(int **beginIndex**, int **length**, Endian **byteEndian** = Endian.BigEndian, System.Text.Encoding **encoding** = null)|将可读取数据的从第**beginIndex**个字节开始的**length**个字节按照**byteEndian**的字节序以**encoding**编码方式读取为字符串</br>_注：若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
|public _T_ ReadValueByBitIndex&#60;_T_&#62;(int **beginIndex**, int **length**, Endian **bitEndian** = Endian.SmallEndian)|将可读取数据的从第**beginIndex**个比特开始的**length**个比特按照**bitEndian**的比特序读取为_T_类型的数值</br>_注：支持读取的数值的比特数不能超过**maxBitsLength**，支持读取的类型为：**bool**, **sbyte**, **byte**, **short**, **ushort**, **int**, **uint**, **long**, **ulong**_|
|public _T_ ReadValueByBitIndex&#60;_T_&#62;(int **byteIndex**, int **bitIndex**, int **length**, Endian **bitEndian** = Endian.SmallEndian)|将可读取数据的从第**byteIndex**个字节的第**bitIndex**个比特开始的**length**个比特按照**bitEndian**的比特序读取为_T_类型的数值</br>_注：支持读取的数值的最大比特数与支持类型与前一方法相同_|
|public **string** ReadStringByBitIndex(int **beginIndex**, int **length**, Endian **bitEndian** = Endian.SmallEndian, System.Text.Encoding **encoding** = null)|将可读取数据的从第**beginIndex**个比特开始的**length**个比特按照**bitEndian**的比特序以**encoding**编码方式读取为字符串</br>_注：输入的**length**参数长度必须为8的整数倍；若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
|public **string** ReadStringByBitIndex(int **byteIndex**, int **bitIndex**, int **length**, Endian **bitEndian** = Endian.SmallEndian, System.Text.Encoding **encoding** = null)|将可读取数据的从第**byteIndex**个字节的第**bitIndex**个比特开始的**length**个比特按照**bitEndian**的比特序以**encoding**编码方式读取为字符串</br>_注：输入的**length**参数长度必须为8的整数倍；若输入的**encoding**参数为**null**，将使用系统默认的编码方式**System.Text.Encoding.Default**对数据进行解码_|
###### [<p align="right">返回目录</p>](#本文内容)