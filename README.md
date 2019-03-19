# Apos.History
A C# library that makes it easy to handle undo and redo.

[![Discord](https://img.shields.io/discord/257949867551358987.svg)](https://discord.gg/Wwdb9Cs)

## Documentation

* Coming soon!

## Build

### [NuGet](https://www.nuget.org/packages/Apos.History/) [![NuGet](https://img.shields.io/nuget/v/Apos.History.svg)](https://www.nuget.org/packages/Apos.History/) [![NuGet](https://img.shields.io/nuget/dt/Apos.History.svg)](https://www.nuget.org/packages/Apos.History/)

## Features

* Undo
* Redo
* History from multiple data structures.

## Usage

```csharp
var historyHandler = new HistoryHandler(Option.None<HistoryHandler>());

var listA = new HistoryList<int>(new List<int>(), Option.Some(historyHandler));
var listB = new HistoryList<int>(new List<int>(), Option.Some(historyHandler));

listA.Add(1);
listA.Add(2);
listA.Add(3);

historyHandler.Undo();
historyHandler.Undo();

historyHandler.Redo();

listA.Add(4);

listB.Add(11);
listB.Add(12);

historyHandler.Undo();
historyHandler.Undo();
historyHandler.Undo();
historyHandler.Undo();
historyHandler.Undo();

historyHandler.AutoCommit = false;
listB.Add(1);
listB.Add(2);
listB.Add(3);
historyHandler.Commit();
historyHandler.AutoCommit = true;

//This next undo will work on the 3 last adds at the same time.
historyHandler.Undo();
```
