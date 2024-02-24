# HOWTO : Deserialize mix json structure?

This code expand on the Microsoft site for learning how to deserialize JSON into C# code.

<https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization>

I added some code on how to deal with class _unfriendly_ JSON like this:

```json
...
"MixStructure": [
    [
      "StartString",
      {
        "Key1": "Value1",
        "Key2": "Value2",
        "Key3": ""
      },
      null,
      "AlwaysThereUnlikePreviousOne"
    ]
  ]
...
```

Hope this help you!
