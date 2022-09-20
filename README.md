<br/>
<p align="center">
  <h3 align="center">Suffix Biginteger Utils</h3>

  <p align="center">
    Implementation of suffix strings. Suitable for beautiful display Biginteger with large values.
    <br/>
    <br/>
  </p>
</p>

## Getting Started


### Installation

In Unity, open "Window" -> "Package Manager".  
Click the "+" sign on top left corner -> "Add package from git URL..."  
Paste this: `https://github.com/ArtemVetik/com.agava.suffixbigintegerutils.git#1.1.0`  
See minimum required Unity version in the `package.json` file.  
Find "Samples" in the package window and click the "Import" button. Use it as a guide.  
To update the package, simply add it again while using a different version tag.  

## Usage

**For numbers less than 10000 the suffix is not added. Beginning with the number 10000 the following suffixes are added to number strings:**  
Thousand - K  
One million - M   
Billion - B   
Trillion - T    
Quadrillion - Q   

**For numbers greater than a quadrillion, the following suffixes are added:**   
a...z, excluding k,    
aa...az, ba...bz, ca...cz, ..., zz,   
etc.    

**Usage example:**
```cs
BigInteger value = BigInteger.Parse("182394221");
var suffixValue = value.FormatWithSuffix(); // suffixValue = 182.394M

var suffixValue = "123.345B";
BigInteger value = suffixValue.ToBigIntegerFromSuffixFormat(); // value = 123345678900

SerializeField, SuffixString] private string _value;
SerializeField, SuffixString] private List<string> _valuesList;
```