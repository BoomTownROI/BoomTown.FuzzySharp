# FuzzySharp


## FuzzyWuzzy C# (DotNet) Implementation

Fuzzy string matching for C# based on [FuzzyWuzzy](https://github.com/seatgeek/fuzzywuzzy) Python algorithm. The algorithm uses [Levenshtein distance](https://en.wikipedia.org/wiki/Levenshtein_distance) to calculate similarity between strings.

This project is a port of the [JavaWuzzy](https://github.com/xdrop/fuzzywuzzy) implementation because of the language similarities. 

The FuzzySharp library is a .netstandard 1.0 project. To run the unit tests you will need .netcore 2.0.

Benefits of this project include

* No Dependencies!
* Can be used in full .Net (>4.5) or .Net Core (>1.0) applications!

## Examples

#### Simple Ratio

```csharp
 Fuzzy.Ratio("hello world", "hello word"); 
 95
```

#### Partial Ratio

```csharp
 Fuzzy.PartialRatio("my new test", "these are my new test strings");
 100
```

#### Simple Ratio With String Options

```csharp
 Fuzzy.Ratio("HelLO woRld!!", "hello word", StringOptions.CaseSensitive, StringOptions.PreserveNonAlphaNumeric);
 52
```

#### Token Sort Ratio
```csharp
 Fuzzy.TokenSortPartialRatio("order words out of", "words out of order");
 100
 Fuzzy.TokenSortRatio("order words out of", "word out of order");
 97
```


#### Token Set Ratio
```csharp
 Fuzzy.TokenSetPartialRatio("fuzzy was a bear", "fuzzy fuzzy was a bear");
 100
 Fuzzy.TokenSetRatio("fuzzy was a bear", "fuzzy fuzzy was a bear");
 100
```


#### Weighted Ratio
```c#
 Fuzzy.WeightedRatio("ratios are awesomething things", "ratios solve things and problems");
 65
```


## Credits 
* [SeatGeek](https://github.com/seatgeek/fuzzywuzzy) for coming up with the initial [algorithm](http://chairnerd.seatgeek.com/fuzzywuzzy-fuzzy-string-matching-in-python)
* [XDrop](https://github.com/xdrop) for converting Python to Java 
* Anyone who worked on the original [python-levenshtein](https://github.com/miohtama/python-Levenshtein) project

