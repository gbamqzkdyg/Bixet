# Bixet：应用层数据解析工具集
---
### 本文内容
>#### [1. 项目描述](#项目描述)
>>##### [1-1. 项目介绍](#项目介绍)
>>##### [1-2. 项目依赖](#项目依赖)
>>>###### [1-2-1. 运行环境](#运行环境)
>>>###### [1-2-2. 项目依赖命名空间](#项目依赖命名空间)
>>##### [1-3-3. 使用方法](#使用方法)
>#### [2. BReader类](#BReader类)

---
## 项目描述
### 项目介绍
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bixet是一组工具集合，为用户提供对以字节流或比特流形式表示的数据进行操作的若干功能。不同的功能被实现为不同的类以供用户使用。本项目计划支持的功能及其最新实现版本如下所示：

|类名|功能|最新实现版本|
|-|-|-|
|__BReader__|从数据指定字节位置/比特位置处依照指定字节序/比特序读取指定长度的数值或字符串|0.3.0|
|__BWriter__|向数据指定字节位置或比特位置处依照指定字节序/比特序写入指定长度的数值或字符串|0.2.0|
|__BTemplete__ & __BBlock__ & __BVariable__|将数据格式描述为形式化模板|0.1.0 & 0.1.0 & 0.1.0|
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

![内存布局]("https://dss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo/logo_white-d0c9fe2af5.png" "内存布局")

## BReader类
<div style="font-size:70%">命名空间：Bixet</div>
<div style="font-size:70%">程序集：Bixet.dll</div>

<br/>
从字节数组的指定位置中读取数值或字符串。

```C#
public class BReader
```